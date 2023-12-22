using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StereoApp.Model;

namespace StereoApp.Presenter.UIHandlers.MainMenu
{
    public abstract class CreatePredefinedFigureMenuHandler : MonoBehaviour
    {
        private PointManager pointManager;
        protected PointManager PointManager =>
            pointManager
            ?? throw new InvalidOperationException(
                "PointManager property can only be used from GenerateFigure()"
            );
        protected abstract Model.SolidFigure GenerateFigure();

        public void ShowFigure()
        {
            pointManager = AppManager.Instance.RecreatePointManager();
            SolidFigure figure;
            try
            {
                figure = GenerateFigure();
            }
            finally
            {
                pointManager = null;
            }

            AppManager.Instance.figure = figure;
            SceneManager.LoadScene("MainScene");
        }

        protected List<Point> CalculateBasePoints(
            int sideCount,
            float radius,
            float offset,
            float height
        )
        {
            var points = new List<Point>();
            var angle = -2 * Mathf.PI / sideCount;
            for (var i = 0; i < sideCount; ++i)
            {
                var x = (float)(radius * Mathf.Sin(i * angle + offset));
                var z = (float)(radius * Mathf.Cos(i * angle + offset));
                points.Add(PointManager.Create(x, height, z));
            }

            return points;
        }
    }
}
