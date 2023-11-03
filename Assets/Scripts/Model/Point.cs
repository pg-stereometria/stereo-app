using System;
using StereoApp.Model.Interfaces;

namespace StereoApp.Model
{
    public class Point : ISerializableTo<Point, SerializedPoint>
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public Point(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
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
            return new Point(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
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
