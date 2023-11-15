using System.Collections.Generic;
using UnityEngine;

namespace StereoApp.UIHandlers.CreatePolygonMenu
{
    public class CreatePolygonMenuHandler : MonoBehaviour
    {
        private const int START_ALPHAPHET = 65;

        [SerializeField]
        private GameObject coordinatesPrefab;

        [SerializeField]
        private GameObject polygonPrefab;

        [SerializeField]
        private GameObject coordinatesParent;

        [SerializeField]
        private float offset = 5.0f;

        private Stack<Coordinates> coordinates;

        private float currentY = 0;
        private int count = 0;

        // Start is called before the first frame update
        private void Start()
        {
            var rt = coordinatesParent.GetComponent<RectTransform>();
            var worldCorners = new Vector3[4];
            rt.GetWorldCorners(worldCorners);
            currentY = worldCorners[1].y; // Get top
            coordinates = new Stack<Coordinates>();
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
            coordinate.title.text = "Point " + (char)(START_ALPHAPHET + count) + ":";
            count++;

            coordinates.Push(coordinate);
        }

        public void OnFinishPressed()
        {
            var points = new List<Model.Point>();
            foreach (var coordinate in coordinates)
            {
                points.Add(
                    new Model.Point(
                        float.Parse(coordinate.xCoordinate.text),
                        float.Parse(coordinate.yCoordinate.text),
                        float.Parse(coordinate.zCoordinate.text)
                    )
                );
            }

            var polygon = new Model.Polygon(points);
            var newPolygon = Instantiate(polygonPrefab);
            newPolygon.GetComponentInParent<Presenter.PolygonPresenter>().Polygon = polygon;
        }
    }
}
