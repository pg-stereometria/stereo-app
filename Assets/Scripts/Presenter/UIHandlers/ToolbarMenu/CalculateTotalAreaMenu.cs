using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using StereoApp.Presenter.Figure;
using StereoApp.Model;

namespace StereoApp.Presenter.UIHandlers.ToolbarMenu
{
    public class CalculateTotalAreaMenu : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text valueText;

        private SolidFigure figure;

        public void FillInTotalArea()
        {
            figure = ToolbarMenuManager.Instance.solidFigurePresenter.Figure;
            valueText.text = figure.TotalArea().ToString("0.## j\u00b2");
        }
    }
}
