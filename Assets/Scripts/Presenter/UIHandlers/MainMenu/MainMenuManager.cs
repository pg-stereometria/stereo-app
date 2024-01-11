using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StereoApp.Presenter.UIHandlers.MainMenu
{
    public class MainMenuManager : MenuManager
    {
        public static MainMenuManager Instance;

        private void Awake()
        {
            AppManager.Instance.points.Clear();
            AppManager.Instance.segments.Clear();

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        [SerializeField]
        private CreatePrismMenuHandler createPrismMenu;

        [SerializeField]
        private CreatePyramidMenuHandler createPyramidMenu;

        [SerializeField]
        private CreateCuboidMenuHandler createCuboidMenu;

        [SerializeField]
        private CreateSphereMenuHandler createSphereMenu;

        [SerializeField]
        private CreateCylinderMenuHandler createCylinderMenu;

        [SerializeField]
        private CreateConeMenuHandler createConeMenu;

        [SerializeField]
        private CreateTruncatedConeMenuHandler createTruncatedConeMenu;

        public void ShowPrismMenu()
        {
            SwitchToMenu(createPrismMenu);
        }

        public void ShowCuboidMenu()
        {
            SwitchToMenu(createCuboidMenu);
        }

        public void ShowPyramidMenu()
        {
            SwitchToMenu(createPyramidMenu);
        }

        public void ShowSphereMenu()
        {
            SwitchToMenu(createSphereMenu);
        }

        public void ShowCylinderMenu()
        {
            SwitchToMenu(createCylinderMenu);
        }

        public void ShowConeMenu()
        {
            SwitchToMenu(createConeMenu);
        }

        public void ShowTruncatedConeMenu()
        {
            SwitchToMenu(createTruncatedConeMenu);
        }

        public override void HideEverythingInToolbar()
        {
            mainMenu.gameObject.SetActive(false);
            createPrismMenu.gameObject.SetActive(false);
            createCuboidMenu.gameObject.SetActive(false);
            createPyramidMenu.gameObject.SetActive(false);
            createSphereMenu.gameObject.SetActive(false);
            createCylinderMenu.gameObject.SetActive(false);
            createConeMenu.gameObject.SetActive(false);
            createTruncatedConeMenu.gameObject.SetActive(false);
        }
    }
}
