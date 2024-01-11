using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using StereoApp.Presenter.Figure;
using StereoApp.Model;

namespace StereoApp.Presenter.UIHandlers.ToolbarMenu
{
    public class CalculateSegmentsMenu : MonoBehaviour
    {
        [SerializeField]
        private Coordinates point1;

        [SerializeField]
        private Coordinates point2;

        [SerializeField]
        private TMP_Text valueText;

        private Polyhedron _polyhedron;

        private void Start()
        {
            if (ToolbarMenuManager.Instance.solidFigurePresenter.Figure is Polyhedron polyhedron)
            {
                _polyhedron = polyhedron;
            }
            else
            {
                _polyhedron = ToolbarMenuManager.Instance.polyhedronPresenter.Figure;
            }

            point1.CurrentSolid = _polyhedron;
            point2.CurrentSolid = _polyhedron;
        }

        public void OnCalculatePressed()
        {
            if (point1.point == null || point2.point == null)
            {
                valueText.text = "---";
                return;
            }

            Segment segment;
            try
            {
                segment = new Segment(point1.point, point2.point);
            }
            catch (ArgumentException)
            {
                // same point -> distance = 0
                valueText.text = "0";
                return;
            }

            valueText.text = segment.GetLength().ToString("0.## j");
        }

        public void ReInitialize()
        {
            point1.Initialize();
            point2.Initialize();
            valueText.text = "---";
        }
    }
}
