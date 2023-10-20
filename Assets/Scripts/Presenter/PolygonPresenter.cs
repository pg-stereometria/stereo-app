using System;
using System.Collections.Specialized;
using StereoApp.Model;
using UnityEngine;

namespace StereoApp.Presenter
{
    public class PolygonPresenter : MonoBehaviour
    {
        private Mesh _mesh;
        private MeshFilter _meshFilter;

        private MeshRenderer _meshRenderer;
        private Polygon _polygon;

        public Polygon Polygon
        {
            get => _polygon;
            set
            {
                if (_polygon != null)
                {
                    _polygon.CollectionChanged -= OnPolygonCollectionChanged;
                }

                _polygon = value;
                if (value != null)
                {
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
            _mesh.Clear();
            Destroy(_meshFilter);
            Destroy(_mesh);
            Destroy(_meshRenderer);
        }

        private void OnPolygonCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
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
            var normals = _mesh.normals;
            var uv = _mesh.uv;
            _mesh.Clear();

            if (vertices.Length != _polygon.Count)
            {
                vertices = new Vector3[_polygon.Count];

                triangles = new int[3 * (_polygon.Count - 2)];

                normals = new Vector3[_polygon.Count];
                Array.Fill(normals, -Vector3.forward);

                // TODO: figure out if we need to do this more correctly
                uv = new Vector2[_polygon.Count];
                Array.Fill(uv, Vector2.zero);
            }

            for (var i = 0; i < _polygon.Count; ++i)
            {
                var point = _polygon[i];
                vertices[i] = new Vector3(point.X, point.Y, point.Z);
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
            _mesh.normals = normals;
            _mesh.uv = uv;
            _meshFilter.mesh = _mesh;
        }
    }
}
