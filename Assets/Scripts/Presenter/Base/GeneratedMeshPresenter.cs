using System.Collections.Generic;
using UnityEngine;

namespace StereoApp.Presenter.Base
{
    public abstract class GeneratedMeshPresenter<TFigure> : FigurePresenter<TFigure>
        where TFigure : class
    {
        protected Mesh mesh;
        protected MeshFilter meshFilter;
        protected MeshRenderer meshRenderer;

        private readonly List<GameObject> gameObjects = new();

        protected override void Start()
        {
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshFilter = gameObject.GetComponent<MeshFilter>();
            mesh = new Mesh();
            base.Start();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (mesh == null)
            {
                return;
            }

            mesh.Clear();
            Destroy(meshFilter);
            Destroy(mesh);
            Destroy(meshRenderer);
        }

        protected override void OnChange()
        {
            base.OnChange();

            if (mesh == null)
            {
                return;
            }

            var figure = Figure;
            if (figure == null)
            {
                mesh.Clear();
                return;
            }

            var vertices = mesh.vertices;
            var triangles = mesh.triangles;
            var uv = mesh.uv;
            mesh.Clear();

            RegenerateMesh(figure, vertices, triangles, uv);
        }

        protected abstract void RegenerateMesh(
            TFigure figure,
            Vector3[] vertices,
            int[] triangles,
            Vector2[] uv
        );

        protected void UpdateMesh(
            Vector3[] vertices,
            int[] triangles,
            Vector2[] uv,
            bool recalculate = true
        )
        {
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;

            if (recalculate)
            {
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();
            }

            meshFilter.mesh = mesh;
        }
    }
}
