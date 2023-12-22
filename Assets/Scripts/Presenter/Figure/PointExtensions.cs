using StereoApp.Model;
using UnityEngine;

namespace StereoApp.Presenter.Figure
{
    public static class PointExtensions
    {
        public static Vector3 ToVector3(this Point point)
        {
            return new Vector3(point.X, point.Y, point.Z);
        }

        public static Point ToPoint(this Vector3 vec)
        {
            return new Point(vec.x, vec.y, vec.z);
        }
    }
}
