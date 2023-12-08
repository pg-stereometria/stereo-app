using System.ComponentModel;
using UnityEngine;

namespace StereoApp.Presenter
{
    public class PointPresenter : MonoBehaviour
    {
        private Model.Point _point;

        public Model.Point Point
        {
            get => _point;
            set
            {
                if (_point != null)
                {
                    _point.PropertyChanged -= OnPointPropertyChanged;
                }

                _point = value;
                if (value != null)
                {
                    value.PropertyChanged += OnPointPropertyChanged;
                }

                if (displayAboveObject != null)
                {
                    displayAboveObject.Text = value?.Label;
                }
            }
        }

        private void OnPointPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var point = (Model.Point)sender;
            if (displayAboveObject != null)
            {
                displayAboveObject.Text = point.Label;
            }
        }

        [SerializeField]
        private DisplayAboveObject displayAboveObject;

        private bool _original = false;

        private void OnDestroy()
        {
            Destroy(displayAboveObject.gameObject);
        }
    }
}
