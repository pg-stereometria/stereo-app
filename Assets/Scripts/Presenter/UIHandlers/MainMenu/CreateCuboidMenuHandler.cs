using StereoApp.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StereoApp.Presenter.Figure;
using UnityEngine.Serialization;

namespace StereoApp.Presenter.UIHandlers.MainMenu
{
    public class CreateCuboidMenuHandler : CreatePredefinedFigureMenuHandler
    {
        [SerializeField]
        TMP_InputField widthInput;

        [SerializeField]
        TMP_InputField depthInput;

        [SerializeField]
        TMP_InputField heightInput;

        protected override SolidFigure GenerateFigure()
        {
            float length = float.Parse(widthInput.text);
            float width = float.Parse(depthInput.text);
            float height = float.Parse(heightInput.text);
            var points = new Dictionary<string, Point>
            {
                { "A", new Point(-length / 2, -height / 2, -width / 2) },
                { "B", new Point(length / 2, -height / 2, -width / 2) },
                { "C", new Point(length / 2, -height / 2, width / 2) },
                { "D", new Point(-length / 2, -height / 2, width / 2) },
                { "E", new Point(-length / 2, height / 2, -width / 2) },
                { "F", new Point(length / 2, height / 2, -width / 2) },
                { "G", new Point(length / 2, height / 2, width / 2) },
                { "H", new Point(-length / 2, height / 2, width / 2) },
            };

            var figure = new Polyhedron();
            figure.Faces.Add(new Polygon(points["A"], points["B"], points["C"], points["D"]));
            figure.Faces.Add(new Polygon(points["E"], points["F"], points["G"], points["H"]));
            figure.Faces.Add(new Polygon(points["A"], points["B"], points["F"], points["E"]));
            figure.Faces.Add(new Polygon(points["A"], points["E"], points["H"], points["D"]));
            figure.Faces.Add(new Polygon(points["C"], points["D"], points["H"], points["G"]));
            figure.Faces.Add(new Polygon(points["B"], points["C"], points["G"], points["F"]));
            AppManager.Instance.longestDistance = Vector3.Distance(
                new Vector3(0, 0, 0),
                points["A"].ToVector3()
            );
            AppManager.Instance.midpoint = figure.CalculateMidpoint().ToVector3();

            return figure;
        }
    }
}
