using System.Collections.Generic;
using StereoApp.Model;
using StereoApp.Presenter.Figure;
using UnityEngine;

namespace StereoApp
{
    public class AppManager : MonoBehaviour
    {
        public static AppManager Instance { get; set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            Screen.fullScreen = false;
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        public PointManager RecreatePointManager()
        {
            pointManager = new PointManager();
            return pointManager;
        }

        public PointManager pointManager = new();
        public Model.SolidFigure figure;
        public readonly HashSet<Segment> segments = new HashSet<Segment>();
        public readonly HashSet<Point> points = new HashSet<Point>();
        public Vector3 midpoint = new Vector3(0, 0, 0);
        public float scale;
    }
}
