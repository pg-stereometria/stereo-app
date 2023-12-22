using StereoApp.Presenter.Figure;
using TMPro;
using UnityEngine;

namespace StereoApp.Presenter.UIHandlers.ToolbarMenu
{
    public class CreateFacesMenuHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject buttonPrefab;

        [SerializeField]
        private RectTransform facesParent;

        [SerializeField]
        private float offset = 5.0f;

        private FaceButtonHandler lastButton;

        private float currentY = 25;

        private int faceCount = 0;

        // Start is called before the first frame update
        private void Start()
        {
            if (
                ToolbarMenuManager.Instance.solidFigurePresenter.Figure
                is Model.Polyhedron polyhedron
            )
            {
                ToolbarMenuManager.Instance.polygonMenu.CurrentPolyhedron = polyhedron;
                foreach (var face in polyhedron.Faces)
                {
                    AddButtonForPolygon(face);
                }
            }
            else
            {
                ToolbarMenuManager.Instance.polyhedronPresenter.Figure = new Model.Polyhedron();
                ToolbarMenuManager.Instance.polygonMenu.CurrentPolyhedron = ToolbarMenuManager
                    .Instance
                    .polyhedronPresenter
                    .Figure;
            }

            var worldCorners = new Vector3[4];
            facesParent.GetWorldCorners(worldCorners);
            currentY = 0;
        }

        // Update is called once per frame
        private void Update() { }

        public void OnAddFacePressed()
        {
            ToolbarMenuManager.Instance.polygonMenu.Clear();
            ToolbarMenuManager.Instance.ShowPolygonMenu();
        }

        private void AddFace()
        {
            var newGameObject = Instantiate(
                buttonPrefab,
                facesParent.TransformPoint(new Vector3(facesParent.rect.width / 2, currentY, 0)),
                Quaternion.identity,
                facesParent
            );

            var rt = newGameObject.GetComponent<RectTransform>();
            currentY -= rt.rect.height + offset;
            if (Mathf.Abs(currentY) > facesParent.rect.height)
            {
                facesParent.sizeDelta = new Vector2(
                    facesParent.sizeDelta.x,
                    facesParent.sizeDelta.y + rt.rect.height + offset
                );
            }

            faceCount++;
            lastButton = newGameObject.GetComponent<FaceButtonHandler>();
        }

        public void AddButtonForPolygon(Model.Polygon polygon)
        {
            AddFace();
            lastButton.polygon = polygon;
        }
    }
}
