using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace StereoApp.Presenter.UIHandlers.ToolbarMenu
{
    public class CreatePolygonMenuHandler : MonoBehaviour
    {
        private const int ALPHABET_START = 'A';

        [SerializeField]
        private GameObject coordinatesPrefab;

        [SerializeField]
        private GameObject polygonPrefab;

        [SerializeField]
        private GameObject coordinatesParent;

        [SerializeField]
        private TMP_InputField inputLabel;

        [SerializeField]
        private float offset = 5.0f;

        public Model.Polyhedron CurrentPolyhedron { get; set; }
        public Model.Polygon CurrentPolygon { get; set; }

        private Stack<Coordinates> coordinates;

        private float currentY = 0;
        private int count = 0;

        public void Clear()
        {
            // Murder all children
            foreach (Transform child in coordinatesParent.transform)
            {
                Destroy(child.gameObject);
            }

            SetDefaultValues();
        }

        public void FillInDataFromPolygon(Model.Polygon polygon)
        {
            CurrentPolygon = polygon;
            foreach (var point in polygon)
            {
                AddNewPoint();
                coordinates.Peek().SelectPoint(point);
            }
            inputLabel.text = polygon.Label;
        }

        public void OnAddPointPressed()
        {
            AddNewPoint();
        }

        public void OnDeletePointPressed()
        {
            if (count == 0)
            {
                return;
            }

            var lastPoint = coordinates.Pop().gameObject;
            count--;
            currentY += lastPoint.GetComponent<RectTransform>().rect.height + offset;
            Destroy(lastPoint);
        }

        private void AddNewPoint()
        {
            var newGameObject = Instantiate(
                coordinatesPrefab,
                new Vector3(0, currentY, 0),
                Quaternion.identity,
                coordinatesParent.transform
            );
            var coordinate = newGameObject.GetComponent<Coordinates>();

            currentY -= newGameObject.GetComponent<RectTransform>().rect.height + offset;
            coordinate.CurrentSolid = CurrentPolyhedron;
            count++;
            coordinates.Push(coordinate);
        }

        public void OnFinishPressed()
        {
            var points = new List<Model.Point>();
            foreach (var coordinate in coordinates.Reverse())
            {
                if (coordinate.point != null)
                {
                    coordinate.point.X = float.Parse(coordinate.xCoordinate.text);
                    coordinate.point.Y = float.Parse(coordinate.yCoordinate.text);
                    coordinate.point.Z = float.Parse(coordinate.zCoordinate.text);
                    points.Add(coordinate.point);
                    continue;
                }

                points.Add(
                    AppManager.Instance.pointManager.Create(
                        float.Parse(coordinate.xCoordinate.text),
                        float.Parse(coordinate.yCoordinate.text),
                        float.Parse(coordinate.zCoordinate.text)
                    )
                );
            }

            if (CurrentPolygon == null)
            {
                var polygon = new Model.Polygon(points);
                polygon.Label = inputLabel.text;
                CurrentPolyhedron.Faces.Add(polygon);
                ToolbarMenuManager.Instance.facesMenu.SetPolygonForLastButton(polygon);
            }
            else
            {
                CurrentPolygon.Label = inputLabel.text;
                CurrentPolygon.ReplaceAll(points);
            }
            CurrentPolygon = null;
            ToolbarMenuManager.Instance.GoBack();
        }

        private void SetDefaultValues()
        {
            count = 0;
            var rt = coordinatesParent.GetComponent<RectTransform>();
            var worldCorners = new Vector3[4];
            rt.GetWorldCorners(worldCorners);
            currentY = worldCorners[1].y; // Get top
            coordinates = new Stack<Coordinates>();
            inputLabel.text = "";
        }
    }
}
