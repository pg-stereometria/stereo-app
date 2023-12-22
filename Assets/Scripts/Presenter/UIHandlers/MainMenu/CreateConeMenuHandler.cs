using StereoApp.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StereoApp.Presenter.Figure;

namespace StereoApp.Presenter.UIHandlers.MainMenu
{
    public class CreateConeMenuHandler : CreatePredefinedFigureMenuHandler
    {
        [SerializeField]
        TMP_InputField radiusInput;

        [SerializeField]
        TMP_InputField heightInput;

        protected override SolidFigure GenerateFigure()
        {
            float radius = float.Parse(radiusInput.text);
            float height = float.Parse(heightInput.text);
            var figure = new Cone(new Circle(radius), height);
            AppManager.Instance.longestDistance = Mathf.Max(1.5f * radius, height / 2);
            AppManager.Instance.midpoint = new Vector3(0, height / 2, 0);

            return figure;
        }
    }
}
