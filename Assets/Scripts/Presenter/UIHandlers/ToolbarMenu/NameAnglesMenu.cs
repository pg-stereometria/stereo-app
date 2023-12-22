using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using StereoApp.Presenter.Figure;
using StereoApp.Model;

namespace StereoApp.Presenter.UIHandlers.ToolbarMenu
{
    public class NameAnglesMenu : MonoBehaviour
    {
        [SerializeField]
        private Coordinates point1;

        [SerializeField]
        private Coordinates middlePoint;

        [SerializeField]
        private Coordinates point2;

        [SerializeField]
        private GameObject anglePrefab;

        [SerializeField]
        private TMP_InputField valueText;

        private List<AnglePresenter> anglePresenters = new List<AnglePresenter>();

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

        public void OnPointDropdownChange()
        {
            var angle = FindAngle();
            valueText.text = angle == null ? "" : angle.Label;
        }

        public void OnFinishPressed()
        {
            var angle = FindAngle();
            if (angle != null)
            {
                angle.Label = valueText.text;
                ToolbarMenuManager.Instance.GoBack();
                return;
            }

            var gameObj = Instantiate(
                anglePrefab,
                middlePoint.point.ToVector3(),
                Quaternion.identity
            );
            angle = gameObj.GetComponent<AnglePresenter>();
            angle.point1 = point1.point;
            angle.middlePoint = middlePoint.point;
            angle.point2 = point2.point;
            angle.Label = valueText.text;
            anglePresenters.Add(angle);
            ToolbarMenuManager.Instance.GoBack();
        }

        public AnglePresenter FindAngle()
        {
            if (point1.point == null || point2.point == null)
            {
                return null;
            }
            foreach (var angle in anglePresenters)
            {
                if (angle.middlePoint != middlePoint.point)
                {
                    continue;
                }

                if (angle.point1 == point1.point && angle.point2 == point2.point)
                {
                    return angle;
                }

                if (angle.point1 == point2.point && angle.point2 == point1.point)
                {
                    return angle;
                }
            }

            return null;
        }
    }
}
