using StereoApp.Presenter.UIHandlers.ToolbarMenu;
using UnityEngine;
using System.Collections.Generic;

namespace StereoApp.Presenter.UIHandlers
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance;

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

        public CreateFacesUIHandler facesMenu;
        public CreatePolygonMenuHandler polygonMenu;
        public RectTransform mainMenu;
        public RectTransform nameThingsMenu;
        public NameSegmentsMenu nameSegmentsMenu;
        public NameAnglesMenu nameAnglesMenu;

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
            HideEverythingInToolbar();
            LastMenus.Push(Current);
            Current = facesMenu.gameObject;
            facesMenu.gameObject.SetActive(true);
        }

        public void ShowPolygonMenu()
        {
            HideEverythingInToolbar();
            LastMenus.Push(Current);
            Current = polygonMenu.gameObject;
            polygonMenu.gameObject.SetActive(true);
        }

        public void ShowNameThingsMenu()
        {
            HideEverythingInToolbar();
            LastMenus.Push(Current);
            Current = nameThingsMenu.gameObject;
            nameThingsMenu.gameObject.SetActive(true);
        }

        public void ShowNameSegmentsMenu()
        {
            HideEverythingInToolbar();
            LastMenus.Push(Current);
            Current = nameSegmentsMenu.gameObject;
            nameSegmentsMenu.gameObject.SetActive(true);
        }

        public void ShowNameAnglesMenu()
        {
            HideEverythingInToolbar();
            LastMenus.Push(Current);
            Current = nameAnglesMenu.gameObject;
            nameAnglesMenu.gameObject.SetActive(true);
        }

        public void GoBack()
        {
            if (LastMenus.Count == 0)
            {
                Current = mainMenu.gameObject;
                return;
            }
            HideEverythingInToolbar();
            Current = LastMenus.Pop();
            Current.SetActive(true);
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
