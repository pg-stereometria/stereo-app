using StereoApp.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StereoApp.Presenter.Figure;

namespace StereoApp.Presenter.UIHandlers.MainMenu
{
    public class CreateSphereMenuHandler : CreatePredefinedFigureMenuHandler
    {
        [SerializeField]
        TMP_InputField radiusInput;
        protected override SolidFigure GenerateFigure()
        {
            float radius = float.Parse(radiusInput.text);
            var figure = new Sphere(radius);
            AppManager.Instance.longestDistance = 1.5f*radius;
            AppManager.Instance.midpoint = new Vector3(0, 0, 0);

            return figure;
        }
    }
}
