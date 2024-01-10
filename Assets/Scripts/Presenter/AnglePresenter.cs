using StereoApp.Presenter.Figure;
using System.ComponentModel;
using UnityEngine;

namespace StereoApp.Presenter
{
    public class AnglePresenter : MonoBehaviour
    {
        private Model.Point _point1;
        public Model.Point Point1
        {
            get => _point1;
            set
            {
                if (_point1 != null)
                {
                    _point1.PropertyChanged -= OnPointChange;
                }

                if (value != null)
                {
                    value.PropertyChanged += OnPointChange;
                }

                _point1 = value;
            }
        }
        private Model.Point _middlePoint;
        public Model.Point MiddlePoint
        {
            get => _middlePoint;
            set
            {
                if (_middlePoint != null)
                {
                    _middlePoint.PropertyChanged -= OnPointChange;
                }

                if (value != null)
                {
                    value.PropertyChanged += OnPointChange;
                }

                _middlePoint = value;
            }
        }
        private Model.Point _point2;
        public Model.Point Point2
        {
            get => _point2;
            set
            {
                if (_point2 != null)
                {
                    _point2.PropertyChanged -= OnPointChange;
                }

                if (value != null)
                {
                    value.PropertyChanged += OnPointChange;
                }

                _point2 = value;
            }
        }
        public string _label;
        public string Label
        {
            get => _label;
            set
            {
                _label = value;
                displayAboveObject.Text = value;
            }
        }

        [SerializeField]
        LineRenderer lineRenderer1;

        [SerializeField]
        LineRenderer lineRenderer2;

        [SerializeField]
        private DisplayAboveObject displayAboveObject;

        [SerializeField]
        private float offsetValue = 0.2f;

        [SerializeField]
        private int steps = 100;

        [SerializeField]
        private float radius = 0.3f;

        private Vector3 _vectorPerpendicular;
        private Vector3 _vector1;
        private Vector3 _vector2;
        private Vector3 _vector3;
        private Vector3 _dirVector;
        private float _angleValue;

        private void Start()
        {
            Initialize();
        }

        private void OnPointChange(object sender, PropertyChangedEventArgs e)
        {
            Initialize();
        }

        public void Initialize()
        {
            _vector1 = Point1.ToPosition() - MiddlePoint.ToPosition();
            _vector2 = Point2.ToPosition() - MiddlePoint.ToPosition();
            _vector3 = Point1.ToPosition() - Point2.ToPosition();

            _vectorPerpendicular = Vector3.Cross(_vector1, _vector2);
            _vectorPerpendicular.Normalize();
            _dirVector = _vector1.normalized + _vector2.normalized;

            displayAboveObject.forwardVector = _vectorPerpendicular;
            displayAboveObject.offset = _dirVector * offsetValue;

            _angleValue = Mathf.Acos(
                (_vector1.sqrMagnitude + _vector2.sqrMagnitude - _vector3.sqrMagnitude)
                    / (2 * _vector1.magnitude * _vector2.magnitude)
            );

            DrawCircle();
            transform.rotation = Quaternion.LookRotation(_vectorPerpendicular, _dirVector);
        }

        private void DrawCircle()
        {
            var partOfCircle = _angleValue / (2 * Mathf.PI);
            var count = (int)Mathf.Abs(partOfCircle * steps);
            lineRenderer1.positionCount = count;
            lineRenderer2.positionCount = count;
            var stepValue = _angleValue / count;
            float circumferenceProgress = 0.0f;
            int currentStep = 0;
            do
            {
                float xScaled = Mathf.Cos(circumferenceProgress - _angleValue / 2 + Mathf.PI / 2);
                float yScaled = Mathf.Sin(circumferenceProgress - _angleValue / 2 + Mathf.PI / 2);

                float x = xScaled * radius;
                float y = yScaled * radius;

                Vector3 currentPosition = new Vector3(x, y, 0.0f);

                lineRenderer1.SetPosition(currentStep, currentPosition);
                lineRenderer2.SetPosition(currentStep, currentPosition);

                currentStep++;
                circumferenceProgress += stepValue;
            } while (currentStep < count);
        }

        private void OnDestroy()
        {
            if (displayAboveObject != null)
            {
                Destroy(displayAboveObject.gameObject);
            }
        }
    }
}
