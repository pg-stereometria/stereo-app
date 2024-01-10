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
            var segment = FindSegment();
            if (segment == null)
            {
                return;
            }

            valueText.text = segment.GetLength().ToString("0.##");
        }

        private Segment FindSegment()
        {
            if (point1.point == null || point2.point == null)
            {
                return null;
            }
            foreach (var seg in point1.point.segments)
            {
                if (point2.point.segments.Contains(seg))
                    return seg;
            }
            return null;
        }
    }
}
