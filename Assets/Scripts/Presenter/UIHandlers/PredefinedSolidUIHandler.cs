using System;
using System.Collections.Generic;
using System.Linq;
using StereoApp.Model;
using StereoApp.Presenter.Figure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StereoApp.Presenter.UIHandlers
{
    public class PredefinedSolidUIHandler : MonoBehaviour
    {
        [SerializeField]
        private TMP_Dropdown _shapeTypeDropdown;

        [SerializeField]
        private Slider _sideCountSlider;

        [SerializeField]
        private SolidFigurePresenter _solidPresenter;

        private const double Radius = 5.0;

        public void Start()
        {
            _shapeTypeDropdown.onValueChanged.AddListener(
                delegate
                {
                    OnValueChange();
                }
            );
            _sideCountSlider.onValueChanged.AddListener(
                delegate
                {
                    OnValueChange();
                }
            );
            OnValueChange();
        }

        private void OnValueChange()
        {
            switch (_shapeTypeDropdown.value)
            {
                case 0:
                    GeneratePrism();
                    break;
                case 1:
                    GeneratePyramid();
                    break;
                case 2:
                    GenerateSphere();
                    break;
                case 3:
                    GenerateCylinder();
                    break;
                case 4:
                    GenerateCone();
                    break;
                case 5:
                    GenerateTruncatedCone();
                    break;
            }
        }

        private static Polygon GenerateRegularPolygonBase(int sideCount)
        {
            var pointManager = AppManager.Instance.RecreatePointManager();
            var angle = 2 * Math.PI / sideCount;
            var basePoints = new List<Point>();
            for (var i = 0; i < sideCount; ++i)
            {
                var x = (float)(Radius * Math.Sin(i * angle));
                var z = (float)(Radius * Math.Cos(i * angle));
                basePoints.Add(pointManager.Create(x, 0, z));
            }

            return new Polygon(basePoints);
        }

        private void GeneratePrism()
        {
            var pointManager = AppManager.Instance.RecreatePointManager();
            var bottom = GenerateRegularPolygonBase((int)_sideCountSlider.value);
            var polyhedron = new Polyhedron();
            var offset = pointManager.Create(0, (float)Radius, 0);

            // bases
            polyhedron.Faces.Add(bottom);
            polyhedron.Faces.Add(
                new Polygon(bottom.Select(point => pointManager.Label(point + offset)))
            );

            // lateral faces
            for (var i = 0; i < bottom.Count; ++i)
            {
                polyhedron.Faces.Add(
                    new Polygon(
                        bottom[i],
                        bottom[(i + 1) % bottom.Count],
                        pointManager.Label(bottom[(i + 1) % bottom.Count] + offset),
                        pointManager.Label(bottom[i] + offset)
                    )
                );
            }

            _solidPresenter.Figure = polyhedron;
        }

        private void GeneratePyramid()
        {
            var pointManager = AppManager.Instance.RecreatePointManager();
            var bottom = GenerateRegularPolygonBase((int)_sideCountSlider.value);
            var polyhedron = new Polyhedron();
            var topVertex = pointManager.Create(0, (float)Radius, 0);

            // base
            polyhedron.Faces.Add(bottom);

            // lateral faces
            for (var i = 0; i < bottom.Count; ++i)
            {
                polyhedron.Faces.Add(
                    new Polygon(bottom[i], bottom[(i + 1) % bottom.Count], topVertex)
                );
            }

            _solidPresenter.Figure = polyhedron;
        }

        private void GenerateSphere()
        {
            var sphere = new Sphere((int)_sideCountSlider.value);
            _solidPresenter.Figure = sphere;
        }

        private void GenerateCylinder()
        {
            var radius = _sideCountSlider.value;
            var height = _sideCountSlider.value * 2;
            var figure = new Cylinder(new Circle(radius), height);
            _solidPresenter.Figure = figure;
        }

        private void GenerateCone()
        {
            var radius = _sideCountSlider.value;
            var height = _sideCountSlider.value * 2;
            var figure = new Cone(new Circle(radius), height);
            _solidPresenter.Figure = figure;
        }

        private void GenerateTruncatedCone()
        {
            var radius = _sideCountSlider.value / 2;
            var figure = new TruncatedCone(new Circle(radius), new Circle(6), 5);
            _solidPresenter.Figure = figure;
        }
    }
}
