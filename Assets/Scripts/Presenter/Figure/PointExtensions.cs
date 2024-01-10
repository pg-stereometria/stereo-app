using StereoApp.Model;
using UnityEngine;

namespace StereoApp.Presenter.Figure
{
    public static class PointExtensions
    {
        public static Vector3 ToPosition(this Point point)
        {
            return new Vector3(point.X, point.Y, point.Z);
        }

        public static Point ToPoint(this Vector3 vec)
        {
            return new Point(vec.x, vec.y, vec.z);
        }

        public static Point ToPoint(this Vector3 vec, string label)
        {
            return new Point(vec.x, vec.y, vec.z, label);
        }

        public static Point FromVector3(this PointManager pointManager, Vector3 vec)
        {
            return vec.ToPoint(pointManager.GenerateNextLabel());
        }
    }
}
