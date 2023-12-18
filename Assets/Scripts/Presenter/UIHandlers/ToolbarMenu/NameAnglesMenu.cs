using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using StereoApp.Presenter.Figure;
using StereoApp.Model;

namespace StereoApp.Presenter.UIHandlers.ToolbarMenu
{
    public class NameAnglesMenu : MonoBehaviour
    {
        [SerializeField]
        private Coordinates point1;

        [SerializeField]
        private Coordinates middlePoint;

        [SerializeField]
        private Coordinates point2;

        [SerializeField]
        private GameObject anglePrefab;

        [SerializeField]
        private TMP_InputField valueText;

        [SerializeField]
        private PolyhedronPresenter solidPresenter;

        private void Start()
        {
            point1.CurrentSolid = solidPresenter.Figure;
            middlePoint.CurrentSolid = solidPresenter.Figure;
            point2.CurrentSolid = solidPresenter.Figure;
        }

        public void OnFinishPressed()
        {
            var gameObj = Instantiate(
                anglePrefab,
                middlePoint.point.ToVector3(),
                Quaternion.identity
            );
            var angle = gameObj.GetComponent<AnglePresenter>();
            angle.point1 = point1.point;
            angle.middlePoint = middlePoint.point;
            angle.point2 = point2.point;
            angle.Label = valueText.text;
            MenuManager.Instance.GoBack();
        }
    }
}
