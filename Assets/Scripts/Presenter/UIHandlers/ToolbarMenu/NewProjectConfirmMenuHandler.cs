using UnityEngine;
using UnityEngine.SceneManagement;

namespace StereoApp.Presenter.UIHandlers.ToolbarMenu
{
    public class NewProjectConfirmMenuHandler : MonoBehaviour
    {
        public void OnYesButtonClick()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void OnNoButtonClick()
        {
            ToolbarMenuManager.Instance.GoBack();
        }
    }
}
