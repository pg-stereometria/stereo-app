using System.Collections.Generic;
using UnityEngine;

namespace StereoApp.Presenter.UIHandlers
{
    public abstract class MenuManager : MonoBehaviour
    {
        [SerializeField]
        protected RectTransform backButton;

        [SerializeField]
        protected RectTransform mainMenu;

        public Stack<GameObject> LastMenus { get; set; } = new();

        protected GameObject current;

        protected virtual void Start()
        {
            current = mainMenu.gameObject;
        }

        protected virtual void Update()
        {
            // back button on Android
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                GoBack();
            }
        }

        public void ShowMainMenu()
        {
            SwitchToMenu(mainMenu);
        }

        protected void SwitchToMenu(Component newMenu)
        {
            HideEverythingInToolbar();
            PushMenu(current);
            var obj = newMenu.gameObject;
            current = obj;
            obj.SetActive(true);
        }

        public virtual void GoBack()
        {
            if (LastMenus.Count == 0)
            {
                current = mainMenu.gameObject;
                return;
            }
            HideEverythingInToolbar();
            current = PopMenu();
            current.SetActive(true);
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

        public abstract void HideEverythingInToolbar();
    }
}
