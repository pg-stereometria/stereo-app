using System.Collections.Generic;
using System.ComponentModel;
using StereoApp.Model;
using StereoApp.Presenter.Base;
using UnityEngine;

namespace StereoApp.Presenter.Figure
{
    public class CirclePresenter : FigurePresenter<Circle>
    {
        private const int SECTORS = 32;

        [SerializeField]
        private GameObject _polygonPrefab;

        public override Circle Figure
        {
            set
            {
                var oldFigure = base.Figure;
                if (oldFigure != null)
                {
                    oldFigure.PropertyChanged -= OnFigureChanged;
                }

                if (value != null)
                {
                    value.PropertyChanged += OnFigureChanged;
                }

                base.Figure = value;
            }
        }
        private float _y;
        public float Y
        {
            get => _y;
            set
            {
                _y = value;
                OnChange();
            }
        }

        private void OnFigureChanged(object sender, PropertyChangedEventArgs e)
        {
            OnChange();
        }

        protected override void OnChange()
        {
            base.OnChange();

            var figure = Figure;
            if (figure == null)
            {
                return;
            }

            var radius = figure.Radius;
            var y = Y;

            const float angle = 2 * Mathf.PI / SECTORS;
            var basePoints = new List<Point>();
            for (var i = 0; i < SECTORS; ++i)
            {
                var x = radius * Mathf.Sin(i * angle);
                var z = radius * Mathf.Cos(i * angle);
                basePoints.Add(new Point(x, y, z));
            }

            var polygon = new Polygon(basePoints);

            var polygonObj = Instantiate(_polygonPrefab, transform.parent);
            polygonObj.GetComponent<PolygonPresenter>().Figure = polygon;
            TrackGameObject(polygonObj);
        }
    }
}
