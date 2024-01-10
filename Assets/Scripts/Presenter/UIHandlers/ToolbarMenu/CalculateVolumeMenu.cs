using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using StereoApp.Presenter.Figure;
using StereoApp.Model;

namespace StereoApp.Presenter.UIHandlers.ToolbarMenu
{
    public class CalculateVolumeMenu : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text valueText;

        private SolidFigure figure;

        public void FillInTotalVolume()
        {
            figure = ToolbarMenuManager.Instance.solidFigurePresenter.Figure;
            valueText.text = figure.Volume().ToString("0.## j\u00b3");
        }
    }
}
