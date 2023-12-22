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

        private void Awake()
        {
            if (AppManager.Instance == null || AppManager.Instance.figure == null)
            {
                presenter.Figure = new Model.Polyhedron();
                return;
            }

            presenter.Figure = AppManager.Instance.figure;
            CameraMovement cameraMovement = Camera.main.GetComponent<CameraMovement>();
            cameraMovement.radius = 3 * AppManager.Instance.longestDistance;
            cameraMovement.centrePoint = AppManager.Instance.midpoint;
        }
    }
}
