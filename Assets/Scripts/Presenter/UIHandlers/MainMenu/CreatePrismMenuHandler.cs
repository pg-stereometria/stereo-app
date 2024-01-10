using StereoApp.Model;
using StereoApp.Presenter.Figure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StereoApp.Presenter.UIHandlers.MainMenu
{
    public class CreatePrismMenuHandler : CreatePredefinedFigureMenuHandler
    {
        [SerializeField]
        TMP_InputField heightInput;

        [SerializeField]
        TMP_InputField lengthInput;

        [SerializeField]
        Slider numberOfSidesInput;

        protected override SolidFigure GenerateFigure()
        {
            var sideCount = (int)numberOfSidesInput.value;
            var height = float.Parse(heightInput.text);
            var length = float.Parse(lengthInput.text);
            var radius = length / (2 * Mathf.Sin(Mathf.PI / sideCount));

            List<Point> bottom = CalculateBasePoints(sideCount, radius, -height / 2);
            List<Point> top = CalculateBasePoints(sideCount, radius, height / 2);

            var figure = new Polyhedron();
            figure.Faces.Add(new Polygon(bottom));
            figure.Faces.Add(new Polygon(top));

            // lateral faces
            for (var i = 0; i < sideCount; ++i)
            {
                figure.Faces.Add(
                    new Polygon(
                        bottom[i],
                        bottom[(i + 1) % bottom.Count],
                        top[(i + 1) % bottom.Count],
                        top[i]
                    )
                );
            }
            Vector3 midpoint = figure.CalculateMidpoint().ToPosition();
            AppManager.Instance.midpoint = midpoint;
            return figure;
        }
    }
}
