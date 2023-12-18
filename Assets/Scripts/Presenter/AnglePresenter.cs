using StereoApp.Presenter.Figure;
using System.ComponentModel;
using UnityEngine;

namespace StereoApp.Presenter
{
    public class AnglePresenter : MonoBehaviour
    {
        public Model.Point point1;
        public Model.Point middlePoint;
        public Model.Point point2;
        public string Label { get; set; }

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

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            var segmentMiddle = (point1.ToVector3() + point2.ToVector3()) / 2;
            _dirVector = segmentMiddle - middlePoint.ToVector3();
            _dirVector.Normalize();
            displayAboveObject.Text = Label;
            displayAboveObject.offset = _dirVector * offsetValue;

            _vector1 = middlePoint.ToVector3() - point1.ToVector3();
            _vector2 = middlePoint.ToVector3() - point2.ToVector3();
            _vector3 = point1.ToVector3() - point2.ToVector3();

            _vectorPerpendicular = Vector3.Cross(_vector1, _vector2);
            _vectorPerpendicular.Normalize();

            displayAboveObject.forwardVector = _vectorPerpendicular;
            DrawCircle();
            transform.rotation = Quaternion.LookRotation(_vectorPerpendicular, _dirVector);
        }

        private void DrawCircle()
        {
            var angleValue = Mathf.Acos(
                (_vector1.sqrMagnitude + _vector2.sqrMagnitude - _vector3.sqrMagnitude)
                    / (2 * _vector1.magnitude * _vector2.magnitude)
            );
            var partOfCircle = angleValue / (2 * Mathf.PI);
            var count = (int)(partOfCircle * steps);
            lineRenderer1.positionCount = count;
            lineRenderer2.positionCount = count;
            var stepValue = angleValue / count;
            float circumferenceProgress = 0.0f;
            int currentStep = 0;
            do
            {
                float xScaled = Mathf.Cos(circumferenceProgress + angleValue / 2);
                float yScaled = Mathf.Sin(circumferenceProgress + angleValue / 2);

                float x = xScaled * radius;
                float y = yScaled * radius;

                Vector3 currentPosition = new Vector3(x, y, 0.0f);

                lineRenderer1.SetPosition(currentStep, currentPosition);
                lineRenderer2.SetPosition(currentStep, currentPosition);

                currentStep++;
                circumferenceProgress += stepValue;
            } while (circumferenceProgress < angleValue);
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
