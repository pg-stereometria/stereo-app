using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StereoApp.Presenter.UIHandlers.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        public static MainMenuManager Instance;

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

        [SerializeField]
        private RectTransform backButton;
        [SerializeField]
        private RectTransform mainMenu;
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
        private CreateTrancatedConeMenuHandler createTrunatedConeMenu;

        public Stack<GameObject> LastMenus { get; set; }

        private GameObject Current;
        private void Start()
        {
            LastMenus = new Stack<GameObject>();
            Current = mainMenu.gameObject;
        }

        public void ShowMainMenu()
        {
            SwitchToMenu(mainMenu);
        }

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

        public void ShowTrancatedConeMenu()
        {
            SwitchToMenu(createTrunatedConeMenu);
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
            mainMenu.gameObject.SetActive(false);
            createPrismMenu.gameObject.SetActive(false);
            createCuboidMenu.gameObject.SetActive(false);
            createPyramidMenu.gameObject.SetActive(false);
            createSphereMenu.gameObject.SetActive(false);
            createCylinderMenu.gameObject.SetActive(false);
            createConeMenu.gameObject.SetActive(false);
            createTrunatedConeMenu.gameObject.SetActive(false);
        }
    }
}
