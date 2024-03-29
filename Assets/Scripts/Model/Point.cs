using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StereoApp.Model.Interfaces;
using UnityEngine;

namespace StereoApp.Model
{
    public class Point : ISerializableTo<Point, SerializedPoint>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
        private float _x;
        public float X
        {
            get => _x;
            set
            {
                _x = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Location));
            }
        }
        private float _y;
        public float Y
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Location));
            }
        }
        private float _z;
        public float Z
        {
            get => _z;
            set
            {
                _z = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Location));
            }
        }
        public (float, float, float) Location => (_x, _y, _z);

        public Point(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point(float x, float y, float z, string label)
            : this(x, y, z)
        {
            Label = label;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public SerializedPoint ToSerializable()
        {
            return new SerializedPoint
            {
                x = X,
                y = Y,
                z = Z
            };
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.Label);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.Label);
        }

        public static Point operator /(Point a, float d)
        {
            return new Point(a.X / d, a.Y / d, a.Z / d, a.Label);
        }

        public override string ToString()
        {
            return Label
                + "("
                + X.ToString("0.##")
                + ","
                + Y.ToString("0.##")
                + ","
                + Z.ToString("0.##")
                + ")";
        }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }
    }

    [Serializable]
    public class SerializedPoint : ISerializableFrom<SerializedPoint, Point>
    {
        public float x;
        public float y;
        public float z;

        public Point ToActualType()
        {
            return new Point(x, y, z);
        }
    }
}
