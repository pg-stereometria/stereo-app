using StereoApp.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StereoApp.Presenter.Figure;

namespace StereoApp.Presenter.UIHandlers.MainMenu
{
    public class CreateCylinderMenuHandler : CreatePredefinedFigureMenuHandler
    {
        [SerializeField]
        TMP_InputField radiusInput;
        [SerializeField]
        TMP_InputField heightInput;
        protected override SolidFigure GenerateFigure()
        {
            float radius = float.Parse(radiusInput.text);
            float height = float.Parse(heightInput.text);
            var figure = new Cylinder(new Circle(radius), height);
            AppManager.Instance.longestDistance = 2*radius;
            AppManager.Instance.midpoint = new Vector3(0, height/2, 0);

            return figure;
        }
    }
}
