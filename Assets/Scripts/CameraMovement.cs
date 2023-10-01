using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private Transform centrePoint;

    private float _width;
    private float _height;

    private float _currentRadius;
    private float _currentPolar;
    private float _currentElevation;
    private Vector3 _newPosition;

    void Start()
    {
        _width = Screen.width / 2f;
        _height = Screen.height / 2f;
        CalculateStartingSphericalCoordinates();
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
            UpdateCartesian();
            transform.position = _newPosition;
            transform.LookAt(centrePoint, Vector3.up);
        }
    }

    private void HandleZoom(Touch touch)
    {
        Touch touchOne = Input.GetTouch(1);
        Vector2 touchZeroPreviousPosition = touch.position - touch.deltaPosition;
        Vector2 touchOnePreviousPosition = touchOne.position - touchOne.deltaPosition;
        float prevTouchDeltaMag = (touchZeroPreviousPosition - touchOnePreviousPosition).magnitude;
        float TouchDeltaMag = (touch.position - touchOne.position).magnitude;
        float deltaMagDiff = prevTouchDeltaMag - TouchDeltaMag;
        _currentRadius += deltaMagDiff * Time.deltaTime;
    }

    private void MoveCamera(Touch touch)
    {
        // Move the cube if the screen has the finger moving.
        if (touch.phase == TouchPhase.Moved)
        {
            Vector2 pos = touch.deltaPosition;
            _currentPolar += -pos.x * speed * Time.deltaTime;
            _currentElevation += -pos.y * speed * Time.deltaTime;
        }
    }

    private void UpdateCartesian()
    {
        float a = _currentRadius * Mathf.Cos(_currentElevation);
        _newPosition.x = a * Mathf.Cos(_currentPolar);
        _newPosition.y = _currentRadius * Mathf.Sin(_currentElevation);
        _newPosition.z = a * Mathf.Sin(_currentPolar);
    }

    private void CalculateStartingSphericalCoordinates()
    {
        Vector3 cartCoords = transform.position;
        if (cartCoords.x == 0)
            cartCoords.x = Mathf.Epsilon;
        _currentRadius = Mathf.Sqrt(
            (cartCoords.x * cartCoords.x)
                + (cartCoords.y * cartCoords.y)
                + (cartCoords.z * cartCoords.z)
        );
        _currentPolar = Mathf.Atan(cartCoords.z / cartCoords.x);
        if (cartCoords.x < 0)
            _currentPolar += Mathf.PI;
        _currentElevation = Mathf.Asin(cartCoords.y / _currentRadius);
    }
}
