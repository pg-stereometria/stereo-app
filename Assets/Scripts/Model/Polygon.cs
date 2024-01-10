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

        public IEnumerable<Segment> Segments =>
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
                Debug.Assert(IsCoplanar(point));
            }
#endif
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

        private bool IsCoplanar(Point point)
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

            if (!IsCoplanar(item))
            {
                throw new ArgumentException("The point is not coplanar with existing points.");
            }

            _points.Add(item);
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

            var value = _points.Remove(item);
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item)
            );
            return value;
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

            if (!IsCoplanar(item))
            {
                throw new ArgumentException("The point is not coplanar with existing points.");
            }

            _points.Insert(index, item);
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
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item)
            );
        }

        public void ReplaceAll(IEnumerable<Point> points)
        {
            var oldItems = _points;
            _points = new List<Point>(points);
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
                        isCoplanar = IsCoplanar(value);
                        (_points[index], _points[3]) = (_points[3], _points[index]);
                    }
                    else
                    {
                        isCoplanar = IsCoplanar(value);
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

        public List<List<Point>> GetTriangles()
        {
            List<List<Point>> triangles = new List<List<Point>>();
            for (int i = 0; i < _points.Count - 2; i++)
            {
                List<Point> triangle = new List<Point>();
                triangle.Add(_points[0]);
                triangle.Add(_points[i + 1]);
                triangle.Add(_points[i + 2]);
                triangles.Add(triangle);
            }
            return triangles;
        }

        public float CalculateArea()
        {
            var triangles = GetTriangles();
            float area = 0.0f;

            foreach (var triangle in triangles)
            {
                area += CalculateAreaOfTriangle(triangle);
            }
            return area;
        }

        private float CalculateAreaOfTriangle(List<Point> triangle)
        {
            var vector1 = triangle[1].ToVector3() - triangle[0].ToVector3();
            var vector2 = triangle[2].ToVector3() - triangle[0].ToVector3();
            return (float)0.5 * Vector3.Cross(vector1, vector2).magnitude;
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
