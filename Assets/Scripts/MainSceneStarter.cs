using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StereoApp.Presenter.Figure;

namespace StereoApp
{
    public class MainSceneStarter : MonoBehaviour
    {
        [SerializeField]
        private SolidFigurePresenter presenter;

        [SerializeField]
        private Vector3 targetViewPort = new Vector3(1.0f, 2.0f, 1.0f);

        private void Awake()
        {
            if (AppManager.Instance == null || AppManager.Instance.figure == null)
            {
                presenter.Figure = new Model.Polyhedron();
                return;
            }
            var midpoint = AppManager.Instance.midpoint;
            var figure = AppManager.Instance.figure;
            if (figure is Model.Polyhedron polyhedron)
            {
                CaclulateScaleForPolyhedron(polyhedron);
            }
            else if (figure is Model.Sphere sphere)
            {
                AppManager.Instance.scale = Mathf.Min(
                    targetViewPort.x / sphere.Radius,
                    targetViewPort.y / sphere.Radius,
                    targetViewPort.z / sphere.Radius
                );
            }
            else if (figure is Model.Cone cone)
            {
                AppManager.Instance.scale = Mathf.Min(
                    targetViewPort.x / cone.BottomBase.Radius,
                    targetViewPort.y / (cone.Height - midpoint.y),
                    targetViewPort.z / cone.BottomBase.Radius
                );
            }
            else if (figure is Model.Cylinder cylinder)
            {
                AppManager.Instance.scale = Mathf.Min(
                    targetViewPort.x / cylinder.BottomBase.Radius,
                    targetViewPort.y / (cylinder.Height - midpoint.y),
                    targetViewPort.z / cylinder.BottomBase.Radius
                );
            }
            else if (figure is Model.TruncatedCone tranctatedCone)
            {
                AppManager.Instance.scale = Mathf.Min(
                    targetViewPort.x / tranctatedCone.BottomBase.Radius,
                    targetViewPort.y / (tranctatedCone.Height - midpoint.y),
                    targetViewPort.z / tranctatedCone.BottomBase.Radius
                );
            }

            presenter.Figure = figure;
            CameraMovement cameraMovement = Camera.main.GetComponent<CameraMovement>();
            cameraMovement.centrePoint = AppManager.Instance.midpoint * AppManager.Instance.scale;
        }

        private void CaclulateScaleForPolyhedron(Model.Polyhedron polyhedron)
        {
            float maxDiff = float.MinValue;
            float currentScale = 1;
            foreach (var point in polyhedron.Points)
            {
                var vector = point.ToVector3() - AppManager.Instance.midpoint;
                var scales = new Vector3(1, 1, 1);
                if (vector.x != 0)
                    scales.x = targetViewPort.x / Mathf.Abs(vector.x);
                if (vector.y != 0)
                    scales.y = targetViewPort.y / Mathf.Abs(vector.y);
                if (vector.z != 0)
                    scales.z = targetViewPort.z / Mathf.Abs(vector.z);

                if (1 - scales.x > maxDiff)
                {
                    maxDiff = 1 - scales.x;
                    currentScale = scales.x;
                }
                if (1 - scales.y > maxDiff)
                {
                    maxDiff = 1 - scales.y;
                    currentScale = scales.y;
                }
                if (1 - scales.z > maxDiff)
                {
                    maxDiff = 1 - scales.z;
                    currentScale = scales.z;
                }
            }
            AppManager.Instance.scale = currentScale;
        }
    }
}
