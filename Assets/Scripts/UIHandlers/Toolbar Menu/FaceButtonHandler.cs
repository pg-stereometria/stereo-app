using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StereoApp.UIHandlers
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
