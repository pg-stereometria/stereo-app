using StereoApp.Presenter.UIHandlers.ToolbarMenu;
using UnityEngine;

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

        [SerializeField]
        private RectTransform menuButton;

        [SerializeField]
        private RectTransform toolbarMenu;

        private CameraMovement cameraMovement;

        private void Start()
        {
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
            facesMenu.gameObject.SetActive(true);
        }

        public void ShowPolygonMenu()
        {
            HideEverythingInToolbar();
            polygonMenu.gameObject.SetActive(true);
        }

        public void HideEverythingInToolbar()
        {
            facesMenu.gameObject.SetActive(false);
            polygonMenu.gameObject.SetActive(false);
        }
    }
}
