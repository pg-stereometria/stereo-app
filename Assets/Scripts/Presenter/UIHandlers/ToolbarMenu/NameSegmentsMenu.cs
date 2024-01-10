using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using StereoApp.Presenter.Figure;
using StereoApp.Model;

namespace StereoApp.Presenter.UIHandlers.ToolbarMenu
{
    public class NameSegmentsMenu : MonoBehaviour
    {
        [SerializeField]
        private Coordinates point1;

        [SerializeField]
        private Coordinates point2;

        [SerializeField]
        private TMP_InputField valueText;

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

        public void OnPointDropdownChange()
        {
            var segment = FindSegment();
            valueText.text = segment?.Label ?? "";
        }

        public void OnFinishPressed()
        {
            var segment = FindSegment();
            if (segment == null)
            {
                return;
            }

            segment.Label = valueText.text;
            ToolbarMenuManager.Instance.GoBack();
        }

        private Segment FindSegment()
        {
            if (point1.point == null || point2.point == null)
            {
                return null;
            }

            var toFind = new Segment(point1.point, point2.point);
            AppManager.Instance.segments.TryGetValue(toFind, out var toReturn);
            return toReturn;
        }
    }
}
