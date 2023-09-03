using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentCreator : MonoBehaviour
{
    [SerializeField]
    private Transform _segmentsParent;

    [SerializeField]
    private GameObject _segmentPrefab;

    private Point _startingPoint;
    private Transform _currentSegment;

    public static SegmentCreator Instance { get; private set; }

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

    public void StartCreatingSegment(Point originPoint)
    {
        _currentSegment = Instantiate(_segmentPrefab, _segmentsParent).transform;
        _currentSegment.position = originPoint.transform.position;
        EditSpaceController.Instance.CurrentMode = Mode.CONNECT_POINTS;
        _startingPoint = originPoint;
        originPoint.IsSelected = true;
    }

    public void StopCreatingSegment(Point endPoint)
    {
        if (_startingPoint == endPoint)
            return;
        TrackSegment(endPoint.transform);
        _currentSegment.localScale = new Vector3(
            _currentSegment.localScale.x,
            Vector3.Distance(endPoint.transform.position, _currentSegment.position) / 2,
            _currentSegment.localScale.z
        );
        _currentSegment = null;
        EditSpaceController.Instance.CurrentMode = Mode.CREATE_POINT;
        _startingPoint.IsSelected = false;
    }

    public void TrackSegment(Transform toFollow)
    {
        _currentSegment.LookAt(toFollow);
        _currentSegment.RotateAround(_currentSegment.position, _currentSegment.right, 90);
        _currentSegment.localScale = new Vector3(
            _currentSegment.localScale.x,
            Vector3.Distance(toFollow.position, _currentSegment.position) / 2,
            _currentSegment.localScale.z
        );
    }
}
