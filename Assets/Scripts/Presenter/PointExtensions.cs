using UnityEngine;

namespace StereoApp.Presenter
{
    public static class PointExtensions
    {
        public static Vector3 ToVector3(this Model.Point point)
        {
            return new Vector3(point.X, point.Y, point.Z);
        }
    }
}
