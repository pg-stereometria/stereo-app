using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StereoApp.UIHandlers.ToolbarMenu
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
        private float offset = 5.0f;

        public Model.Polyhedron CurrentSolid { get; set; }
        public Model.Polygon CurrentPolygon { get; set; }

        private Stack<Coordinates> coordinates;

        private float currentY = 0;
        private int count = 0;

        // Start is called before the first frame update
        private void Start()
        {
            SetDefaultValues();
        }

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
            coordinate.CurrentSolid = CurrentSolid;
            count++;

            coordinates.Push(coordinate);
        }

        public void OnFinishPressed()
        {
            var points = new List<Model.Point>();
            var pointsCount = CurrentSolid.Points.Count;
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
                    new Model.Point(
                        float.Parse(coordinate.xCoordinate.text),
                        float.Parse(coordinate.yCoordinate.text),
                        float.Parse(coordinate.zCoordinate.text),
                        ((char)(ALPHABET_START + pointsCount + points.Count)).ToString()
                    )
                );
            }

            if (CurrentPolygon == null)
            {
                var polygon = new Model.Polygon(points);
                CurrentSolid.Faces.Add(polygon);
                MenuManager.Instance.facesMenu.SetPolygonForLastButton(polygon);
            }
            else
            {
                CurrentPolygon.ReplaceAll(points);
                CurrentPolygon = null;
            }

            MenuManager.Instance.ShowFacesMenu();
        }

        private void SetDefaultValues()
        {
            count = 0;
            var rt = coordinatesParent.GetComponent<RectTransform>();
            var worldCorners = new Vector3[4];
            rt.GetWorldCorners(worldCorners);
            currentY = worldCorners[1].y; // Get top
            coordinates = new Stack<Coordinates>();
        }
    }
}
