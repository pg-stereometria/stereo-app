using StereoApp.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StereoApp.Presenter.Figure;

namespace StereoApp.Presenter.UIHandlers.MainMenu
{
    public class CreateTrancatedConeMenuHandler : CreatePredefinedFigureMenuHandler
    {
        [SerializeField]
        TMP_InputField bottomRadiusInput;
        [SerializeField]
        TMP_InputField topRadiusInput;
        [SerializeField]
        TMP_InputField heightInput;
        protected override SolidFigure GenerateFigure()
        {
            float bottomRadius = float.Parse(bottomRadiusInput.text);
            float topRadius = float.Parse(topRadiusInput.text);
            float height = float.Parse(heightInput.text);
            var figure = new TruncatedCone(new Circle(topRadius), new Circle(bottomRadius), height);
            AppManager.Instance.longestDistance = Mathf.Max(1.5f*bottomRadius, 1.5f*topRadius, height);
            AppManager.Instance.midpoint = new Vector3(0, height/2, 0);

            return figure;
        }
    }
}
