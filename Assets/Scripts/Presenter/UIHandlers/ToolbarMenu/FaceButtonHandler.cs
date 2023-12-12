using UnityEngine;

namespace StereoApp.Presenter.UIHandlers.ToolbarMenu
{
    public class FaceButtonHandler : MonoBehaviour
    {
        public Model.Polygon polygon;

        public void EditPolygon()
        {
            MenuManager.Instance.polygonMenu.Clear();
            MenuManager.Instance.polygonMenu.FillInDataFromPolygon(polygon);
            MenuManager.Instance.ShowPolygonMenu();
        }
    }
}
