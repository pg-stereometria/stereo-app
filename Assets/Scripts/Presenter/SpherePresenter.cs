using System.ComponentModel;
using StereoApp.Model;
using StereoApp.Presenter.Base;
using UnityEngine;

namespace StereoApp.Presenter
{
    public class SpherePresenter : GeneratedMeshPresenter<Sphere>
    {
        // number of horizontal (☰) dividers - i.e. parallels on a globe
        private const int PARALLELS = 30;

        // number of vertical (|||) dividers - i.e. meridians (pole to pole) on a globe
        private const int MERIDIANS = 30;

        public override Sphere Figure
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
            Sphere figure,
            Vector3[] vertices,
            int[] triangles,
            Vector2[] uv
        )
        {
            const int verticesDimensionSize = MERIDIANS + 1;
            const int trianglesDimensionSize = 6 * MERIDIANS;
            const int vertexCount = verticesDimensionSize * (PARALLELS + 1);
            const int triangleCount = trianglesDimensionSize * PARALLELS;
            if (vertices.Length != vertexCount)
            {
                vertices = new Vector3[vertexCount];
                triangles = new int[triangleCount];
                uv = new Vector2[vertexCount];
            }

            var radius = figure.Radius;

            var n = 0;
            for (var i = 0; i <= PARALLELS; ++i)
            {
                var longitude = Mathf.PI * 2 * i / PARALLELS;
                for (var j = 0; j <= MERIDIANS; ++j)
                {
                    var latitude = Mathf.PI * j / MERIDIANS - Mathf.PI / 2;
                    var sphericalPoint = new Vector3(
                        radius * Mathf.Cos(longitude) * Mathf.Cos(latitude),
                        radius * Mathf.Sin(latitude),
                        radius * Mathf.Sin(longitude) * Mathf.Cos(latitude)
                    );
                    var index = i * verticesDimensionSize + j;
                    vertices[index] = sphericalPoint;
                    var uvPoint = new Vector2((float)i / PARALLELS, (float)j / MERIDIANS);
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

            UpdateMesh(vertices, triangles, uv);
        }
    }
}
