using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private float minRadius = 1f;

    [SerializeField]
    private float maxRadius = 100f;

    public Vector3 centrePoint;
    public float radius;

    private bool _flipped;
    private float _currentPolar;
    private float _currentElevation;
    private Vector3 _newPosition = new Vector3(0, 0, 0);

    void Start()
    {
        CalculateStartingSphericalCoordinates();
        UpdateTransform();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (Input.touchCount == 2)
            {
                HandleZoom(touch);
            }
            else
            {
                MoveCamera(touch);
            }
            UpdateTransform();
        }
    }

    private void UpdateTransform()
    {
        UpdateCartesian();
        transform.position = _newPosition;

        _flipped = false;
        Vector3 direction = Vector3.up;
        if (_currentElevation > Mathf.PI / 2 || _currentElevation < -Mathf.PI / 2)
        {
            direction = Vector3.down;
            _flipped = true;
        }
        transform.LookAt(centrePoint, direction);
    }

    private void HandleZoom(Touch touch)
    {
        Touch touchOne = Input.GetTouch(1);
        Vector2 touchZeroPreviousPosition = touch.position - touch.deltaPosition;
        Vector2 touchOnePreviousPosition = touchOne.position - touchOne.deltaPosition;
        float prevTouchDeltaMag = (touchZeroPreviousPosition - touchOnePreviousPosition).magnitude;
        float TouchDeltaMag = (touch.position - touchOne.position).magnitude;
        float deltaMagDiff = prevTouchDeltaMag - TouchDeltaMag;
        radius = Mathf.Clamp(radius + deltaMagDiff * Time.deltaTime, minRadius, maxRadius);
    }

    private void MoveCamera(Touch touch)
    {
        // Move the cube if the screen has the finger moving.
        if (touch.phase == TouchPhase.Moved)
        {
            int xMultiplier = 1;
            if (_flipped)
                xMultiplier = -1;
            Vector2 pos = touch.deltaPosition;
            _currentPolar += -pos.x * xMultiplier * speed * Time.deltaTime;
            _currentElevation += -pos.y * speed * Time.deltaTime;

            // polar and elevation should be in range from -PI to PI
            if (_currentPolar > Mathf.PI)
                _currentPolar -= 2 * Mathf.PI;
            if (_currentElevation > Mathf.PI)
                _currentElevation -= 2 * Mathf.PI;
            if (_currentPolar < -Mathf.PI)
                _currentPolar += 2 * Mathf.PI;
            if (_currentElevation < -Mathf.PI)
                _currentElevation += 2 * Mathf.PI;
        }
    }

    private void UpdateCartesian()
    {
        float a = radius * Mathf.Cos(_currentElevation);
        _newPosition.x = a * Mathf.Cos(_currentPolar) + centrePoint.x;
        _newPosition.y = radius * Mathf.Sin(_currentElevation) + centrePoint.y;
        _newPosition.z = a * Mathf.Sin(_currentPolar) + centrePoint.z;
    }

    private void CalculateStartingSphericalCoordinates()
    {
        Vector3 cartCoords = transform.position;
        if (cartCoords.x == 0)
            cartCoords.x = Mathf.Epsilon;
        _currentPolar = Mathf.Atan(cartCoords.z / cartCoords.x);
        if (cartCoords.x < 0)
            _currentPolar += Mathf.PI;
        _currentElevation = Mathf.Asin(cartCoords.y / radius);
    }
}
