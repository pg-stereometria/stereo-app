using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasicsCreator : MonoBehaviour
{
    public enum Mode
    {
        NONE,
        CREATE_POINT,
        CONNECT_POINTS
    }
    public Mode CurrentMode { get; set; } = Mode.CREATE_POINT;
    public bool CanCreatePoints { get; set; } = true;

    [SerializeField]
    private Transform _segmentsParent;
    [SerializeField]
    private Transform _pointsParent;
    [SerializeField] 
    private Transform _mousePoint;
    [SerializeField]
    private GameObject _segmentPrefab;
    [SerializeField]
    private GameObject _pointPrefab;
    [SerializeField]
    private Slider _zPositionSlider;
    [SerializeField]
    private TMP_InputField _xPositionInput;
    [SerializeField]
    private TMP_InputField _yPositionInput;
    [SerializeField]
    private TMP_InputField _zPositionInput;
    [SerializeField]
    private float _minZDistance = 1f;

    private Transform _currentSegment;
    private Vector3 _screenPosition;
    private Vector3 _worldPosition;

    public static BasicsCreator Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        CalculatePosition();
        TrackMouse();

        if (Input.GetMouseButtonDown(0) && CurrentMode != Mode.NONE && CanCreatePoints)
        {
            Point point = CreatePoint(_worldPosition);
            if (CurrentMode == Mode.CONNECT_POINTS)
                StopCreatingSegment(point);
        }

        if (CurrentMode == Mode.CONNECT_POINTS)
            TrackSegment(_mousePoint);
    }

    public void CreatePointFromUI()
    {
        Vector3 position = new Vector3(float.Parse(_xPositionInput.text),
            float.Parse(_yPositionInput.text),
            float.Parse(_zPositionInput.text));
        CreatePoint(position);
    }

    public void StartCreatingSegment(Point originPoint)
    {
        _currentSegment = Instantiate(_segmentPrefab, _segmentsParent).transform;
        _currentSegment.position = originPoint.transform.position;
        CurrentMode = Mode.CONNECT_POINTS;
    }
    public void StopCreatingSegment(Point endPoint)
    {
        TrackSegment(endPoint.transform);
        _currentSegment.localScale = new Vector3(_currentSegment.localScale.x, Vector3.Distance(endPoint.transform.position, _currentSegment.position) / 2, _currentSegment.localScale.z);
        _currentSegment = null;
        CurrentMode = Mode.CREATE_POINT;
    }
    public void TrackSegment(Transform toFollow)
    {
        _currentSegment.LookAt(toFollow);
        _currentSegment.RotateAround(_currentSegment.position, _currentSegment.right, 90);
        _currentSegment.localScale = new Vector3(_currentSegment.localScale.x, Vector3.Distance(toFollow.position, _currentSegment.position) / 2, _currentSegment.localScale.z);
    }

    private void TrackMouse()
    {
        _mousePoint.position = _worldPosition;
    }

    private void CalculatePosition()
    {
        _screenPosition = Input.mousePosition;
        _screenPosition.z = Camera.main.nearClipPlane + _zPositionSlider.value + _minZDistance;
        _worldPosition = Camera.main.ScreenToWorldPoint(_screenPosition);
    }
    private Point CreatePoint(Vector3 positionToCreate)
    {
        return Instantiate(_pointPrefab, positionToCreate, Quaternion.identity, _pointsParent).GetComponent<Point>();
    }
}
