using System;
using System.Collections.Generic;
using System.Linq;
using StereoApp.Model;
using StereoApp.Presenter;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StereoApp.UIHandlers
{
    public class PredefinedSolidUIHandler : MonoBehaviour
    {
        [SerializeField]
        private TMP_Dropdown _shapeTypeDropdown;

        [SerializeField]
        private Slider _sideCountSlider;

        [SerializeField]
        private SolidFigurePresenter _solidPresenter;

        [SerializeField]
        private SpherePresenter _spherePresenter;

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
            }
        }

        private static Polygon GenerateRegularPolygonBase(int sideCount)
        {
            var angle = 2 * Math.PI / sideCount;
            var basePoints = new List<Model.Point>();
            for (var i = 0; i < sideCount; ++i)
            {
                var x = (float)(Radius * Math.Sin(i * angle));
                var z = (float)(Radius * Math.Cos(i * angle));
                basePoints.Add(new Model.Point(x, 0, z));
            }

            return new Polygon(basePoints);
        }

        private void GeneratePrism()
        {
            var bottom = GenerateRegularPolygonBase((int)_sideCountSlider.value);
            var solid = new SolidFigure();
            var offset = new Model.Point(0, (float)Radius, 0);

            // bases
            solid.Add(bottom);
            solid.Add(new Polygon(bottom.Select(point => point + offset)));

            // lateral faces
            for (var i = 0; i < bottom.Count; ++i)
            {
                solid.Add(
                    new Polygon(
                        bottom[i],
                        bottom[(i + 1) % bottom.Count],
                        bottom[(i + 1) % bottom.Count] + offset,
                        bottom[i] + offset
                    )
                );
            }

            _solidPresenter.Solid = solid;
            _spherePresenter.Sphere = null;
        }

        private void GeneratePyramid()
        {
            var bottom = GenerateRegularPolygonBase((int)_sideCountSlider.value);
            var solid = new SolidFigure();
            var topVertex = new Model.Point(0, (float)Radius, 0);

            // base
            solid.Add(bottom);

            // lateral faces
            for (var i = 0; i < bottom.Count; ++i)
            {
                solid.Add(new Polygon(bottom[i], bottom[(i + 1) % bottom.Count], topVertex));
            }

            _solidPresenter.Solid = solid;
            _spherePresenter.Sphere = null;
        }

        private void GenerateSphere()
        {
            var sphere = new Sphere((int)_sideCountSlider.value);
            _solidPresenter.Solid = null;
            _spherePresenter.Sphere = sphere;
        }
    }
}
