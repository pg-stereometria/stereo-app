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

        [SerializeField]
        private PolyhedronPresenter solidPresenter;

        private void Start()
        {
            point1.CurrentSolid = solidPresenter.Figure;
            point2.CurrentSolid = solidPresenter.Figure;
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
            MenuManager.Instance.GoBack();
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
