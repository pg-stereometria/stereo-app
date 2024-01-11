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
        private GameObject _segmentPrefab;
        [SerializeField]
        private GameObject _pointPrefab;

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

        public void ReInitialize()
        {
            point1.Initialize();
            point2.Initialize();
            valueText.text = "";
        }

        public void OnFinishPressed()
        {
            var segment = FindSegment();
            if (segment == null)
            {
                var gameObj = Instantiate(_segmentPrefab);
                segment = new Segment(point1.point, point2.point);
                AppManager.Instance.segments.Add(segment);
                var segmentPresenter = gameObj.GetComponent<SegmentPresenter>();
                segmentPresenter.Figure = segment;
            }

            segment.Label = valueText.text;
            ToolbarMenuManager.Instance.GoBack();
        }

        private Segment FindSegment()
        {
            if (point1.point == null)
            {
                if (point1.xCoordinate.text.Equals("") || point1.yCoordinate.text.Equals("") || point1.zCoordinate.text.Equals(""))
                    return null;
                point1.point = AppManager.Instance.pointManager.Create(float.Parse(point1.xCoordinate.text), float.Parse(point1.yCoordinate.text), float.Parse(point1.zCoordinate.text));
                var newGameObject = Instantiate(
                    _pointPrefab,
                    point1.point.ToPosition(),
                    Quaternion.identity
                );
                newGameObject.GetComponent<PointPresenter>().Figure = point1.point;
            }
            if (point2.point == null)
            {
                if (point2.xCoordinate.text.Equals("") || point2.yCoordinate.text.Equals("") || point2.zCoordinate.text.Equals(""))
                    return null;
                point2.point = AppManager.Instance.pointManager.Create(float.Parse(point2.xCoordinate.text), float.Parse(point2.yCoordinate.text), float.Parse(point2.zCoordinate.text));
                var newGameObject = Instantiate(
                    _pointPrefab,
                    point2.point.ToPosition(),
                    Quaternion.identity
                );
                newGameObject.GetComponent<PointPresenter>().Figure = point2.point;
            }

            var toFind = new Segment(point1.point, point2.point);
            AppManager.Instance.segments.TryGetValue(toFind, out var toReturn);
            return toReturn;
        }
    }
}
