using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using StereoApp.Model.Interfaces;
using UnityEngine;

namespace StereoApp.Model
{
    public class Polygon
        : ISerializableTo<Polygon, SerializedPolygon>,
            IList<Point>,
            INotifyCollectionChanged,
            INotifyPropertyChanged
    {
        private const float CoplanarTolerance = 1e-5f;
        private List<Point> _points;

        public IEnumerable<Segment> Sides =>
            _points.Select((point, i) => new Segment(point, _points[(i + 1) % _points.Count]));

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public int Count => _points.Count;
        public bool IsReadOnly => false;

        public Polygon(params Point[] points)
            : this((IEnumerable<Point>)points) { }

        public Polygon(IEnumerable<Point> points)
        {
            _points = new List<Point>(points);
            if (_points.Count < 3)
            {
                throw new ArgumentException("Polygon needs to contain minimum of 3 points.");
            }
#if DEBUG
            foreach (var point in _points.Skip(3))
            {
                Debug.Assert(IsCoplanarWith(point));
            }
#endif
            if (!IsConvex())
            {
                throw new ArgumentException("The provided points don't make a convex polygon.");
            }
        }

        private string _label = "";
        public string Label
        {
            get => _label;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                _label = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public SerializedPolygon ToSerializable()
        {
            return new SerializedPolygon
            {
                points = _points.Select(point => point.ToSerializable()).ToList()
            };
        }

        public bool IsCoplanarWith(Point point)
        {
            // partial calculations
            var a1 = _points[1].X - _points[0].X;
            var b1 = _points[1].Y - _points[0].Y;
            var c1 = _points[1].Z - _points[0].Z;
            var a2 = _points[2].X - _points[0].X;
            var b2 = _points[2].Y - _points[0].Y;
            var c2 = _points[2].Z - _points[0].Z;
            // calculate constants of the plane equation
            var a = b1 * c2 - b2 * c1;
            var b = a2 * c1 - a1 * c2;
            var c = a1 * b2 - b1 * a2;
            var d = -a * _points[0].X - b * _points[0].Y - c * _points[0].Z;

            // check if the passed point satisfies the equation
            var result = a * point.X + b * point.Y + c * point.Z + d;

            return result < CoplanarTolerance && result > -CoplanarTolerance;
        }

        private bool IsConvex()
        {
            var previousSide = new Segment(_points[^1], _points[0]);
            var angleSum = 0.0f;
            foreach (var side in Sides)
            {
                var v1 = (Vector3)previousSide.First;
                var v2 = (Vector3)previousSide.Second;
                var v3 = (Vector3)side.Second;
                var angle = Vector3.SignedAngle(v2 - v1, v3 - v2, Vector3.up);

                if (angle is 0.0f or 180.0f or -180.0f)
                {
                    // points are collinear
                    return false;
                }

                if (angleSum * angle < 0.0f)
                {
                    // angles have different directions - one of them will be concave
                    return false;
                }

                angleSum += angle;
                previousSide = side;
            }

            // absolute sum of the calculated angles should be approximately equal to 360 degrees,
            // otherwise polygon is self-intersecting and therefore not convex
            return Mathf.Abs(Mathf.Abs(angleSum) - 360.0f) < 5.0f;
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return _points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_points).GetEnumerator();
        }

        public void Add(Point item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }

            if (!IsCoplanarWith(item))
            {
                throw new ArgumentException("The point is not coplanar with existing points.");
            }

            _points.Add(item);
            if (!IsConvex())
            {
                _points.RemoveAt(_points.Count - 1);
                throw new ArgumentException(
                    "The polygon is not convex when the provided point is part of it."
                );
            }

            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add,
                    item,
                    _points.Count - 1
                )
            );
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(Point item)
        {
            return _points.Contains(item);
        }

        public void CopyTo(Point[] array, int arrayIndex)
        {
            _points.CopyTo(array, arrayIndex);
        }

        public bool Remove(Point item)
        {
            if (_points.Count == 3)
            {
                throw new InvalidOperationException(
                    "Polygon needs to contain minimum of 3 points."
                );
            }

            for (var i = 0; i < _points.Count; ++i)
            {
                if (!_points[i].Equals(item))
                {
                    continue;
                }

                _points.RemoveAt(i);
                if (!IsConvex())
                {
                    _points.Insert(i, item);
                    throw new ArgumentException(
                        "The polygon is not convex without the provided point."
                    );
                }

                OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item)
                );
                return true;
            }

            return false;
        }

        public int IndexOf(Point item)
        {
            return _points.IndexOf(item);
        }

        public void Insert(int index, Point item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }

            if (!IsCoplanarWith(item))
            {
                throw new ArgumentException("The point is not coplanar with existing points.");
            }

            _points.Insert(index, item);
            if (!IsConvex())
            {
                _points.RemoveAt(index);
                throw new ArgumentException(
                    "The polygon is not convex when the provided point is part of it."
                );
            }

            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index)
            );
        }

        public void RemoveAt(int index)
        {
            if (_points.Count == 3)
            {
                throw new InvalidOperationException(
                    "Polygon needs to contain minimum of 3 points."
                );
            }

            var item = this[index];
            _points.RemoveAt(index);
            if (!IsConvex())
            {
                _points.Insert(index, item);
                throw new ArgumentException(
                    "The polygon is not convex without the point at provided index."
                );
            }

            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item)
            );
        }

        public void ReplaceAll(IEnumerable<Point> points)
        {
            var oldItems = _points;
            _points = new List<Point>(points);

            if (_points.Skip(3).Any(point => !IsCoplanarWith(point)))
            {
                _points = oldItems;
                throw new ArgumentException("The provided points are not coplanar.");
            }
            if (!IsConvex())
            {
                _points = oldItems;
                throw new ArgumentException("The polygon is not convex with the provided points.");
            }

            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Replace,
                    oldItems,
                    _points.AsReadOnly()
                )
            );
        }

        public Point this[int index]
        {
            get => _points[index];
            set
            {
                if (_points.Count != 3)
                {
                    bool isCoplanar;
                    if (index < 2)
                    {
                        // temporary swap
                        (_points[index], _points[3]) = (_points[3], _points[index]);
                        isCoplanar = IsCoplanarWith(value);
                        (_points[index], _points[3]) = (_points[3], _points[index]);
                    }
                    else
                    {
                        isCoplanar = IsCoplanarWith(value);
                    }

                    if (!isCoplanar)
                    {
                        throw new ArgumentException(
                            "The point is not coplanar with existing points."
                        );
                    }
                }

                var oldValue = _points[index];
                _points[index] = value;

                if (!IsConvex())
                {
                    _points[index] = oldValue;
                    throw new ArgumentException(
                        "The polygon is not convex when the provided point is part of it."
                    );
                }

                OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Replace,
                        value,
                        oldValue
                    )
                );
            }
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
            CollectionChanged?.Invoke(this, e);
        }

        public override string ToString()
        {
            var name = "";
            foreach (var item in _points)
            {
                name += item.Label;
            }

            if (!string.IsNullOrEmpty(Label))
            {
                name += $"({Label})";
            }
            return name;
        }
    }

    [Serializable]
    public class SerializedPolygon : ISerializableFrom<SerializedPolygon, Polygon>
    {
        public List<SerializedPoint> points;

        public Polygon ToActualType()
        {
            return new Polygon(points.Select(point => point.ToActualType()));
        }
    }
}
