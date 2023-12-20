using StereoApp.Presenter.UIHandlers.ToolbarMenu;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using StereoApp.Presenter.Figure;

namespace StereoApp.Presenter.UIHandlers.ToolbarMenu
{
    public class ToolbarMenuManager : MonoBehaviour
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
        public RectTransform mainMenu;
        public RectTransform nameThingsMenu;
        public NameSegmentsMenu nameSegmentsMenu;
        public NameAnglesMenu nameAnglesMenu;

        [SerializeField]
        private RectTransform backButton;

        [SerializeField]
        private RectTransform menuButton;

        [SerializeField]
        private RectTransform toolbarMenu;

        public Stack<GameObject> LastMenus { get; set; }

        private GameObject Current;
        private CameraMovement cameraMovement;

        private void Start()
        {
            LastMenus = new Stack<GameObject>();
            Current = mainMenu.gameObject;
            cameraMovement = Camera.main.GetComponent<CameraMovement>();
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
                cameraMovement.enabled = false;
            }
            else
            {
                menuButton.anchoredPosition = new Vector2(
                    menuButton.anchoredPosition.x,
                    menuButton.anchoredPosition.y - toolbarMenu.rect.height
                );
                toolbarMenu.gameObject.SetActive(false);
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

        private void SwitchToMenu(Component newMenu)
        {
            HideEverythingInToolbar();
            PushMenu(Current);
            var obj = newMenu.gameObject;
            Current = obj;
            obj.SetActive(true);
        }

        public void GoBack()
        {
            if (LastMenus.Count == 0)
            {
                Current = mainMenu.gameObject;
                return;
            }
            HideEverythingInToolbar();
            Current = PopMenu();
            Current.SetActive(true);
        }

        private void PushMenu(GameObject obj)
        {
            backButton.gameObject.SetActive(true);
            LastMenus.Push(obj);
        }

        private GameObject PopMenu()
        {
            backButton.gameObject.SetActive(LastMenus.Count != 1);
            return LastMenus.Pop();
        }

        public void HideEverythingInToolbar()
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
