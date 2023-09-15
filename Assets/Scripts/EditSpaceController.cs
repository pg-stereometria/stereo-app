using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Mode
{
    NONE,
    POINT_MANIPULATION,
}

public class EditSpaceController : MonoBehaviour
{
    private Mode _currentMode { get; set; } = Mode.POINT_MANIPULATION;
    private bool _isMouseOverPoint { get; set; } = false;

    [SerializeField]
    private Transform _mousePoint;

    [SerializeField]
    private Slider _zPositionSlider;

    [SerializeField]
    private float _minZDistance = 1f;

    private Camera _mainCamera;

    private Vector3 _screenPosition;
    private Vector3 _worldPosition;

    public static EditSpaceController Instance { get; private set; }

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

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        CalculatePosition();
        TrackMouse();

        switch (_currentMode)
        {
            case Mode.NONE:
                break;
            case Mode.POINT_MANIPULATION:
                HandlePointManipulation();
                break;
        }
    }

    void HandlePointManipulation()
    {
        if (_isMouseOverPoint)
        {
            return;
        }

        if (SegmentCreator.Instance.StartingPoint is null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PointCreator.Instance.CreatePoint(_worldPosition);
            }
            return;
        }

        SegmentCreator.Instance.TrackSegment(_mousePoint);
        if (Input.GetMouseButtonUp(0))
        {
            var point = PointCreator.Instance.CreatePoint(_worldPosition);
            SegmentCreator.Instance.StopCreatingSegment(point);
        }
    }

    private void TrackMouse()
    {
        _mousePoint.position = _worldPosition;
    }

    private void CalculatePosition()
    {
        _screenPosition = Input.mousePosition;
        _screenPosition.z = _mainCamera.nearClipPlane + _zPositionSlider.value + _minZDistance;
        _worldPosition = _mainCamera.ScreenToWorldPoint(_screenPosition);
    }

    public void OnPointMouseEnter(Point point)
    {
        SegmentCreator.Instance.TrackSegment(point.transform);
        _isMouseOverPoint = true;
    }

    public void OnPointMouseExit(Point point)
    {
        _isMouseOverPoint = false;
    }

    public void OnPointMouseOver(Point point)
    {
        if (SegmentCreator.Instance.StartingPoint is null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SegmentCreator.Instance.StartCreatingSegment(point);
            }

            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            SegmentCreator.Instance.StopCreatingSegment(point);
        }
    }
}
