using System.Collections.Specialized;
using System.ComponentModel;
using StereoApp.Model;
using StereoApp.Presenter.Base;
using UnityEngine;

namespace StereoApp.Presenter.Figure
{
    public class PolygonPresenter : GeneratedMeshPresenter<Polygon>
    {
        [SerializeField]
        private GameObject _segmentPrefab;

        [SerializeField]
        private GameObject _pointPrefab;

        [SerializeField]
        private bool _renderPoints = true;

        [SerializeField]
        private DisplayAboveObject displayAboveObject;

        public override Polygon Figure
        {
            set
            {
                var oldFigure = base.Figure;
                if (oldFigure != null)
                {
                    foreach (var point in oldFigure)
                    {
                        point.PropertyChanged -= OnPointChanged;
                    }

                    oldFigure.PropertyChanged -= OnFigureChanged;
                    oldFigure.CollectionChanged -= OnPolygonCollectionChanged;
                }

                if (value != null)
                {
                    foreach (var point in value)
                    {
                        point.PropertyChanged += OnPointChanged;
                    }

                    value.PropertyChanged += OnFigureChanged;
                    value.CollectionChanged += OnPolygonCollectionChanged;
                }

                if (displayAboveObject != null)
                {
                    displayAboveObject.Text = value?.Label;
                }

                base.Figure = value;
            }
        }

        private void OnFigureChanged(object sender, PropertyChangedEventArgs e)
        {
            if (displayAboveObject != null)
            {
                displayAboveObject.Text = Figure.Label;
            }
        }

        private void OnPointChanged(object sender, PropertyChangedEventArgs e)
        {
            OnChange();
        }

        private void OnPolygonCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Point point in e.NewItems)
                    {
                        point.PropertyChanged += OnPointChanged;
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (Point point in e.OldItems)
                    {
                        point.PropertyChanged -= OnPointChanged;
                    }

                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (Point point in e.OldItems)
                    {
                        point.PropertyChanged -= OnPointChanged;
                    }

                    foreach (Point point in e.NewItems)
                    {
                        point.PropertyChanged += OnPointChanged;
                    }

                    break;
            }

            OnChange();
        }

        protected override void RegenerateMesh(
            Polygon figure,
            Vector3[] vertices,
            int[] triangles,
            Vector2[] uv
        )
        {
            var renderPoints = _renderPoints;
            if (vertices.Length != figure.Count)
            {
                vertices = new Vector3[figure.Count];
                triangles = new int[3 * (figure.Count - 2)];
            }

            for (var i = 0; i < figure.Count; ++i)
            {
                var point = figure[i];
                var vertex = point.ToPosition();
                vertices[i] = vertex;
                if (!renderPoints)
                {
                    continue;
                }

                var newGameObject = Instantiate(
                    _pointPrefab,
                    vertex,
                    Quaternion.identity,
                    transform
                );
                newGameObject.GetComponent<PointPresenter>().Figure = point;
                TrackGameObject(newGameObject);
            }

            foreach (var segment in figure.Segments)
            {
                var gameObj = Instantiate(_segmentPrefab, transform);

                var firstVertex = segment.First.ToPosition();
                var secondVertex = segment.Second.ToPosition();

                var t = gameObj.transform;
                t.position = firstVertex;
                t.LookAt(secondVertex);
                t.RotateAround(t.position, t.right, 90);

                var localScale = t.localScale;
                localScale.y = Vector3.Distance(secondVertex, firstVertex) / 2;
                t.localScale = localScale;

                AppManager.Instance.segments.TryGetValue(segment, out var existing);
                if (existing == null)
                {
                    AppManager.Instance.segments.Add(segment);
                    existing = segment;
                }
                var segmentPresenter = gameObj.GetComponent<SegmentPresenter>();
                segmentPresenter.Figure = existing;

                TrackGameObject(gameObj);
            }

            // simple fan triangulation - only guaranteed to work on convex polygons
            for (var i = 0; i < figure.Count - 2; ++i)
            {
                var baseIndex = 3 * i;
                triangles[baseIndex] = 0;
                triangles[baseIndex + 1] = i + 1;
                triangles[baseIndex + 2] = i + 2;
            }

            UpdateMesh(vertices, triangles, uv, false);

            var sum = new Vector3(0, 0, 0);
            foreach (var pointCoordinates in vertices)
            {
                sum += pointCoordinates;
            }
            var midpoint = sum / vertices.Length;
            displayAboveObject.offset = midpoint - transform.position;
            displayAboveObject.forwardVector = Vector3
                .Cross(vertices[1] - vertices[0], vertices[1] - vertices[2])
                .normalized;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (displayAboveObject != null)
            {
                Destroy(displayAboveObject.gameObject);
            }
        }
    }
}
