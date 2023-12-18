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

        public void OnFinishPressed()
        {
            Segment segment;
            segment = FindSegment();
            if (segment == null)
            {
                return;
            }

            segment.Label = valueText.text;
            MenuManager.Instance.GoBack();
        }

        private Segment FindSegment()
        {
            foreach (var seg in point1.point.segments)
            {
                if (point2.point.segments.Contains(seg))
                    return seg;
            }
            return null;
        }
    }
}
