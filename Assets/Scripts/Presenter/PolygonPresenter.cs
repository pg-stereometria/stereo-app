using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using StereoApp.Model;
using UnityEngine;

namespace StereoApp.Presenter
{
    public class PolygonPresenter : MonoBehaviour
    {
        [SerializeField]
        private GameObject _segmentPrefab;

        [SerializeField]
        private GameObject _pointPrefab;

        private Mesh _mesh;
        private MeshFilter _meshFilter;

        private MeshRenderer _meshRenderer;
        private Polygon _polygon;

        private readonly List<GameObject> _gameObjects = new();

        public Polygon Polygon
        {
            get => _polygon;
            set
            {
                if (_polygon != null)
                {
                    foreach (var point in _polygon)
                    {
                        point.PropertyChanged -= OnPointChanged;
                    }

                    _polygon.CollectionChanged -= OnPolygonCollectionChanged;
                }

                _polygon = value;
                if (value != null)
                {
                    foreach (var point in value)
                    {
                        point.PropertyChanged += OnPointChanged;
                    }

                    value.CollectionChanged += OnPolygonCollectionChanged;
                }

                UpdateMesh();
            }
        }

        public void Start()
        {
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();

            _meshFilter = gameObject.GetComponent<MeshFilter>();
            _mesh = new Mesh();
            UpdateMesh();
        }

        public void OnDestroy()
        {
            CleanupSegments();
            _mesh.Clear();
            Destroy(_meshFilter);
            Destroy(_mesh);
            Destroy(_meshRenderer);
        }

        private void OnPointChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateMesh();
        }

        private void OnPolygonCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Model.Point point in e.NewItems)
                    {
                        point.PropertyChanged += OnPointChanged;
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (Model.Point point in e.OldItems)
                    {
                        point.PropertyChanged -= OnPointChanged;
                    }

                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (Model.Point point in e.OldItems)
                    {
                        point.PropertyChanged -= OnPointChanged;
                    }

                    foreach (Model.Point point in e.NewItems)
                    {
                        point.PropertyChanged += OnPointChanged;
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    UpdateMesh();
                    break;
            }

            UpdateMesh();
        }

        private void UpdateMesh()
        {
            if (_mesh == null)
            {
                return;
            }

            if (_polygon == null)
            {
                _mesh.Clear();
                return;
            }

            var vertices = _mesh.vertices;
            var triangles = _mesh.triangles;
            _mesh.Clear();
            CleanupSegments();

            if (vertices.Length != _polygon.Count)
            {
                vertices = new Vector3[_polygon.Count];
                triangles = new int[3 * (_polygon.Count - 2)];
            }

            for (var i = 0; i < _polygon.Count; ++i)
            {
                var point = _polygon[i];
                var vertex = point.ToVector3();
                vertices[i] = vertex;
                _gameObjects.Add(Instantiate(_pointPrefab, vertex, Quaternion.identity, transform));
            }

            foreach (var segment in _polygon.Segments)
            {
                var gameObj = Instantiate(_segmentPrefab, transform);

                var firstVertex = segment.First.ToVector3();
                var secondVertex = segment.Second.ToVector3();

                var t = gameObj.transform;
                t.position = firstVertex;
                t.LookAt(secondVertex);
                t.RotateAround(t.position, t.right, 90);

                var localScale = t.localScale;
                localScale.y = Vector3.Distance(secondVertex, firstVertex) / 2;
                t.localScale = localScale;

                _gameObjects.Add(gameObj);
            }

            // simple fan triangulation - only guaranteed to work on convex polygons
            for (var i = 0; i < _polygon.Count - 2; ++i)
            {
                var baseIndex = 3 * i;
                triangles[baseIndex] = 0;
                triangles[baseIndex + 1] = i + 1;
                triangles[baseIndex + 2] = i + 2;
            }

            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _meshFilter.mesh = _mesh;
        }

        private void CleanupSegments()
        {
            foreach (var gameObj in _gameObjects)
            {
                Destroy(gameObj);
            }

            _gameObjects.Clear();
        }
    }
}
