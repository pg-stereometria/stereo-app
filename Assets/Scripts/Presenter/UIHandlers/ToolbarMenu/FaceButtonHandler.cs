using UnityEngine;
using TMPro;

namespace StereoApp.Presenter.UIHandlers.ToolbarMenu
{
    public class FaceButtonHandler : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text textField;

        public Model.Polygon polygon;

        private void Update()
        {
            textField.text = polygon.ToString();
        }

        public void EditPolygon()
        {
            ToolbarMenuManager.Instance.polygonMenu.Clear();
            ToolbarMenuManager.Instance.polygonMenu.FillInDataFromPolygon(polygon);
            ToolbarMenuManager.Instance.ShowPolygonMenu();
        }
    }
}
