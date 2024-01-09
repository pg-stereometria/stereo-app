using StereoApp.Presenter.UIHandlers.ToolbarMenu;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using StereoApp.Presenter.Figure;

namespace StereoApp.Presenter.UIHandlers.ToolbarMenu
{
    public class ToolbarMenuManager : MenuManager
    {
        public static ToolbarMenuManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public SolidFigurePresenter solidFigurePresenter;
        public PolyhedronPresenter polyhedronPresenter;

        public CreateFacesMenuHandler facesMenu;
        public CreatePolygonMenuHandler polygonMenu;
        public RectTransform nameThingsMenu;
        public NameSegmentsMenu nameSegmentsMenu;
        public NameAnglesMenu nameAnglesMenu;

        [SerializeField]
        private RectTransform menuButton;

        [SerializeField]
        private RectTransform toolbarMenu;

        private CameraMovement cameraMovement;
        private Camera mainCamera;
        private float menuOpenCameraOffset => toolbarMenu.rect.height / Screen.height;

        protected override void Start()
        {
            base.Start();
            mainCamera = Camera.main;
            cameraMovement = mainCamera.GetComponent<CameraMovement>();
        }

        public void OnMenuButtonPressed()
        {
            if (!toolbarMenu.gameObject.activeSelf)
            {
                menuButton.anchoredPosition = new Vector2(
                    menuButton.anchoredPosition.x,
                    menuButton.anchoredPosition.y + toolbarMenu.rect.height
                );
                toolbarMenu.gameObject.SetActive(true);
                mainCamera.transform.position -= menuOpenCameraOffset * mainCamera.transform.up;
                cameraMovement.enabled = false;
            }
            else
            {
                menuButton.anchoredPosition = new Vector2(
                    menuButton.anchoredPosition.x,
                    menuButton.anchoredPosition.y - toolbarMenu.rect.height
                );
                toolbarMenu.gameObject.SetActive(false);
                mainCamera.transform.position += menuOpenCameraOffset * mainCamera.transform.up;
                cameraMovement.enabled = true;
            }
        }

        public void ShowFacesMenu()
        {
            SwitchToMenu(facesMenu);
        }

        public void ShowPolygonMenu()
        {
            SwitchToMenu(polygonMenu);
        }

        public void ShowNameThingsMenu()
        {
            SwitchToMenu(nameThingsMenu);
        }

        public void ShowNameSegmentsMenu()
        {
            SwitchToMenu(nameSegmentsMenu);
        }

        public void ShowNameAnglesMenu()
        {
            SwitchToMenu(nameAnglesMenu);
        }

        public override void HideEverythingInToolbar()
        {
            facesMenu.gameObject.SetActive(false);
            polygonMenu.gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(false);
            nameThingsMenu.gameObject.SetActive(false);
            nameSegmentsMenu.gameObject.SetActive(false);
            nameAnglesMenu.gameObject.SetActive(false);
        }
    }
}
