using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Mode
{
    NONE,
    CREATE_POINT,
    CONNECT_POINTS
}

public class EditSpaceController : MonoBehaviour
{
    public Mode CurrentMode { get; set; } = Mode.CREATE_POINT;
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

        if (Input.GetMouseButtonDown(0) && CurrentMode != Mode.NONE && !_isMouseOverPoint)
        {
            var point = PointCreator.Instance.CreatePoint(_worldPosition);
            if (CurrentMode == Mode.CONNECT_POINTS)
                SegmentCreator.Instance.StopCreatingSegment(point);
        }
        if (CurrentMode == Mode.CONNECT_POINTS)
        {
            SegmentCreator.Instance.TrackSegment(_mousePoint);
            if (Input.GetMouseButtonUp(0) && !_isMouseOverPoint)
            {
                var point = PointCreator.Instance.CreatePoint(_worldPosition);
                SegmentCreator.Instance.StopCreatingSegment(point);
            }
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
        if (CurrentMode == Mode.CONNECT_POINTS)
        {
            SegmentCreator.Instance.TrackSegment(point.transform);
        }
        _isMouseOverPoint = true;
    }

    public void OnPointMouseExit(Point point)
    {
        _isMouseOverPoint = false;
    }

    public void OnPointMouseOver(Point point)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (CurrentMode == Mode.CONNECT_POINTS)
            {
                SegmentCreator.Instance.StopCreatingSegment(point);
            }
            else
            {
                SegmentCreator.Instance.StartCreatingSegment(point);
            }
        }
        else if (Input.GetMouseButtonUp(0) && CurrentMode == Mode.CONNECT_POINTS)
        {
            SegmentCreator.Instance.StopCreatingSegment(point);
        }
    }
}
