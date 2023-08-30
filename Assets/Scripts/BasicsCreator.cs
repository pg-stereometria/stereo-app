using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Mode
{
    NONE,
    CREATE_POINT,
    CONNECT_POINTS
}

public class BasicsCreator : MonoBehaviour
{
    public Mode CurrentMode { get; set; } = Mode.CREATE_POINT;
    public bool CanCreatePoints { get; set; } = true;

    [SerializeField]
    private Transform _mousePoint;

    [SerializeField]
    private Slider _zPositionSlider;

    [SerializeField]
    private float _minZDistance = 1f;

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
            Point point = PointCreator.Instance.CreatePoint(_worldPosition);
            if (CurrentMode == Mode.CONNECT_POINTS)
                SegmentCreator.Instance.StopCreatingSegment(point);
        }
        if (CurrentMode == Mode.CONNECT_POINTS)
        {
            SegmentCreator.Instance.TrackSegment(_mousePoint);
            if (Input.GetMouseButtonUp(0) && CanCreatePoints)
            {
                Point point = PointCreator.Instance.CreatePoint(_worldPosition);
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
        _screenPosition.z = Camera.main.nearClipPlane + _zPositionSlider.value + _minZDistance;
        _worldPosition = Camera.main.ScreenToWorldPoint(_screenPosition);
    }
}
