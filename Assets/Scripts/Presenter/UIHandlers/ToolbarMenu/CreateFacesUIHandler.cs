using StereoApp.Presenter.Figure;
using TMPro;
using UnityEngine;

namespace StereoApp.Presenter.UIHandlers.ToolbarMenu
{
    public class CreateFacesUIHandler : MonoBehaviour
    {
        [SerializeField]
        private PolyhedronPresenter solidPresenter;

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
            solidPresenter.Figure = new Model.Polyhedron();
            var worldCorners = new Vector3[4];
            facesParent.GetWorldCorners(worldCorners);
            currentY = 0;
        }

        // Update is called once per frame
        private void Update() { }

        public void OnAddFacePressed()
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

            var ButtonText = newGameObject.GetComponentInChildren<TMP_Text>();
            ButtonText.text = "Face " + faceCount + ":";
            MenuManager.Instance.polygonMenu.CurrentSolid = solidPresenter.Figure;
            MenuManager.Instance.polygonMenu.Clear();
            MenuManager.Instance.ShowPolygonMenu();
        }

        public void SetPolygonForLastButton(Model.Polygon polygon)
        {
            lastButton.polygon = polygon;
        }
    }
}
