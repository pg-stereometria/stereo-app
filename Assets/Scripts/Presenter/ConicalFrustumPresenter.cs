using System;
using System.ComponentModel;
using StereoApp.Model.Interfaces;
using UnityEngine;

namespace StereoApp.Presenter
{
    public class ConicalFrustumPresenter : MonoBehaviour
    {
        // number of horizontal (☰) dividers - i.e. parallels on a globe
        private const int PARALLELS = 30;

        // number of vertical (|||) dividers - i.e. meridians (pole to pole) on a globe
        private const int MERIDIANS = 30;

        private Mesh _mesh;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        private IConicalFrustum _figure;
        public IConicalFrustum Figure
        {
            get => _figure;
            set
            {
                if (_figure != null)
                {
                    _figure.PropertyChanged -= OnFigureChanged;
                }

                _figure = value;
                if (value != null)
                {
                    value.PropertyChanged += OnFigureChanged;
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

        private void OnFigureChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateMesh();
        }

        private void UpdateMesh()
        {
            if (_mesh == null)
            {
                return;
            }

            if (_figure == null)
            {
                _mesh.Clear();
                return;
            }

            var vertices = _mesh.vertices;
            var triangles = _mesh.triangles;
            var uv = _mesh.uv;
            _mesh.Clear();

            const int verticesDimensionSize = MERIDIANS;
            const int trianglesDimensionSize = 6 * (MERIDIANS - 1);
            const int vertexCount = verticesDimensionSize * (PARALLELS + 1);
            const int triangleCount = trianglesDimensionSize * PARALLELS;
            if (vertices.Length != vertexCount)
            {
                vertices = new Vector3[vertexCount];
                triangles = new int[triangleCount];
                uv = new Vector2[vertexCount];
            }

            var n = 0;
            for (var i = 0; i <= PARALLELS; ++i)
            {
                var longitude = Mathf.PI * 2 * i / PARALLELS;
                for (var j = 0; j < MERIDIANS; ++j)
                {
                    var fraction = (float)j / (MERIDIANS - 1);
                    var radius = Mathf.Lerp(
                        _figure.BottomBase.Radius,
                        _figure.TopBase.Radius,
                        fraction
                    );

                    var conicalPoint = new Vector3(
                        radius * Mathf.Cos(longitude),
                        Mathf.Lerp(0.0f, _figure.Height, fraction),
                        radius * Mathf.Sin(longitude)
                    );

                    var index = i * verticesDimensionSize + j;
                    vertices[index] = conicalPoint;
                    var uvPoint = new Vector2((float)i / PARALLELS, (float)j / (MERIDIANS - 1));
                    uv[index] = uvPoint;

                    if (i > 0 && j > 0)
                    {
                        var baseIndex = (i - 1) * trianglesDimensionSize + (j - 1) * 6;
                        triangles[baseIndex] = n;
                        triangles[baseIndex + 1] = n - MERIDIANS - 1;
                        triangles[baseIndex + 2] = n - MERIDIANS;
                        triangles[baseIndex + 3] = n;
                        triangles[baseIndex + 4] = n - 1;
                        triangles[baseIndex + 5] = n - MERIDIANS - 1;
                    }

                    ++n;
                }
            }

            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.uv = uv;

            _mesh.RecalculateBounds();
            _mesh.RecalculateNormals();
            _meshFilter.mesh = _mesh;
        }
    }
}
