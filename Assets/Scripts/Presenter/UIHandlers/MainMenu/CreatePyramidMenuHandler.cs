using StereoApp.Model;
using StereoApp.Presenter.Figure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StereoApp.Presenter.UIHandlers.MainMenu
{
    public class CreatePyramidMenuHandler : CreatePredefinedFigureMenuHandler
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
            var length = float.Parse(lengthInput.text);
            var height = float.Parse(heightInput.text);
            var radius = length / (2 * Mathf.Sin(Mathf.PI / sideCount));



            var offset = Mathf.PI + Mathf.PI / 4;
            List<Point> bottom = CalculateBasePoints(sideCount, radius, offset, -height / 2);

            var figure = new Polyhedron();
            figure.Faces.Add(new Polygon(bottom));
            var topVertex = new Point(figure.CaclulateMidpoint() + new Vector3(0, height, 0));

            // lateral faces
            for (var i = 0; i < sideCount; ++i)
            {
                figure.Faces.Add(
                    new Polygon(
                        bottom[i],
                        bottom[(i + 1) % bottom.Count],
                        topVertex
                    )
                );
            }
            Vector3 midpoint = figure.CaclulateMidpoint();
            AppManager.Instance.longestDistance = Mathf.Max(length, Vector3.Distance(topVertex.ToVector3(), bottom[0].ToVector3()));
            AppManager.Instance.midpoint = midpoint;
            return figure;
        }
    }
}
