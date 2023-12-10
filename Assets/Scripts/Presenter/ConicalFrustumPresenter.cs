using System;
using System.ComponentModel;
using StereoApp.Model.Interfaces;
using StereoApp.Presenter.Base;
using UnityEngine;

namespace StereoApp.Presenter
{
    public class ConicalFrustumPresenter : GeneratedMeshPresenter<IConicalFrustum>
    {
        [SerializeField]
        private GameObject _circlePrefab;

        // number of horizontal (☰) dividers - i.e. parallels on a globe
        private const int PARALLELS = 30;

        // number of vertical (|||) dividers - i.e. meridians (pole to pole) on a globe
        private const int MERIDIANS = 30;

        public override IConicalFrustum Figure
        {
            set
            {
                var oldFigure = base.Figure;
                if (oldFigure != null)
                {
                    oldFigure.PropertyChanged -= OnFigureChanged;
                }

                if (value != null)
                {
                    value.PropertyChanged += OnFigureChanged;
                }

                base.Figure = value;
            }
        }

        private void OnFigureChanged(object sender, PropertyChangedEventArgs e)
        {
            OnChange();
        }

        protected override void RegenerateMesh(
            IConicalFrustum figure,
            Vector3[] vertices,
            int[] triangles,
            Vector2[] uv
        )
        {
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
                        figure.BottomBase.Radius,
                        figure.TopBase.Radius,
                        fraction
                    );

                    var conicalPoint = new Vector3(
                        radius * Mathf.Cos(longitude),
                        Mathf.Lerp(0.0f, figure.Height, fraction),
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

            var height = Figure.Height;
            var bottomBase = Figure.BottomBase;
            var topBase = Figure.TopBase;
            var circleObj = Instantiate(_circlePrefab.gameObject, transform.parent);
            var presenter = circleObj.GetComponent<CirclePresenter>();
            presenter.Figure = bottomBase;
            presenter.Y = 0.0f;
            TrackGameObject(circleObj);

            if (topBase.Valid)
            {
                circleObj = Instantiate(_circlePrefab.gameObject, transform.parent);
                presenter = circleObj.GetComponent<CirclePresenter>();
                presenter.Figure = topBase;
                presenter.Y = height;
                TrackGameObject(circleObj);
            }

            UpdateMesh(vertices, triangles, uv);
        }
    }
}
