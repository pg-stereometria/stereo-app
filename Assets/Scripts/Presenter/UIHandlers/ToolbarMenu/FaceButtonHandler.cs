using UnityEngine;

namespace StereoApp.Presenter.UIHandlers.ToolbarMenu
{
    public class FaceButtonHandler : MonoBehaviour
    {
        public Model.Polygon polygon;

        public void EditPolygon()
        {
            ToolbarMenuManager.Instance.polygonMenu.Clear();
            ToolbarMenuManager.Instance.polygonMenu.FillInDataFromPolygon(polygon);
            ToolbarMenuManager.Instance.ShowPolygonMenu();
        }
    }
}
