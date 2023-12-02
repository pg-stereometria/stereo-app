using UnityEngine;

namespace StereoApp.Presenter
{
    public class PointPresenter : MonoBehaviour
    {
        public Model.Point Point { get; set; }

        [SerializeField]
        private DisplayAboveObject displayAboveObject;

        private bool _original = false;

        private void OnDestroy()
        {
            Destroy(displayAboveObject.gameObject);
        }
    }
}
