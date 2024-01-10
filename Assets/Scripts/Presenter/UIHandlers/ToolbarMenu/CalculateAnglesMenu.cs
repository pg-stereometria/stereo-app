using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using StereoApp.Presenter.Figure;
using StereoApp.Model;

namespace StereoApp.Presenter.UIHandlers.ToolbarMenu
{
    public class CalculateAnglesMenu : MonoBehaviour
    {
        [SerializeField]
        private Coordinates point1;

        [SerializeField]
        private Coordinates middlePoint;

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
            middlePoint.CurrentSolid = _polyhedron;
            point2.CurrentSolid = _polyhedron;
        }

        public void OnCalculatePressed()
        {
            var vector1 = point1.point.ToPosition() - middlePoint.point.ToPosition();
            var vector2 = point2.point.ToPosition() - middlePoint.point.ToPosition();
            var vector3 = point1.point.ToPosition() - point2.point.ToPosition();

            var angleValue = Mathf.Acos(
                (vector1.sqrMagnitude + vector2.sqrMagnitude - vector3.sqrMagnitude)
                    / (2 * vector1.magnitude * vector2.magnitude)
            );
            var angleInDegrees = angleValue * (180 / Mathf.PI);
            valueText.text = Mathf.RoundToInt(angleInDegrees).ToString();
        }
    }
}
