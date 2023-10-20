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
            GeneratePrism(); // if shape type dropdown is prism, else generate a pyramid
        }

        private void GeneratePrism()
        {
            var sideCount = (int)_sideCountSlider.value;
            var radius = 5.0;
            var angle = 2 * Math.PI / sideCount;
            var basePoints = new List<Model.Point>();
            for (var i = 0; i < sideCount; ++i)
            {
                var x = (float)(radius * Math.Sin(i * angle));
                var z = (float)(radius * Math.Cos(i * angle));
                basePoints.Add(new Model.Point(x, 0, z));
            }

            var solid = new SolidFigure();
            var offset = new Model.Point(0, (float)radius, 0);
            solid.Add(new Polygon(basePoints));
            solid.Add(new Polygon(basePoints.Select(point => point + offset)));
            for (var i = 0; i < sideCount; ++i)
            {
                solid.Add(
                    new Polygon(
                        basePoints[i],
                        basePoints[(i + 1) % sideCount],
                        basePoints[(i + 1) % sideCount] + offset,
                        basePoints[i] + offset
                    )
                );
            }

            _solidPresenter.Solid = solid;
        }
    }
}
