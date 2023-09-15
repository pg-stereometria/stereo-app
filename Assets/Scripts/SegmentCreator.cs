using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentCreator : MonoBehaviour
{
    [SerializeField]
    private Transform _segmentsParent;

    [SerializeField]
    private GameObject _segmentPrefab;

    public Point StartingPoint { get; private set; }
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
        if (StartingPoint is not null)
        {
            StartingPoint.IsSelected = false;
            StartingPoint = null;
        }
        _currentSegment = Instantiate(_segmentPrefab, _segmentsParent).transform;
        _currentSegment.position = originPoint.transform.position;
        StartingPoint = originPoint;
        originPoint.IsSelected = true;
    }

    public void StopCreatingSegment(Point endPoint)
    {
        if (StartingPoint is null || StartingPoint == endPoint)
        {
            return;
        }

        TrackSegment(endPoint.transform);
        _currentSegment.localScale = new Vector3(
            _currentSegment.localScale.x,
            Vector3.Distance(endPoint.transform.position, _currentSegment.position) / 2,
            _currentSegment.localScale.z
        );
        _currentSegment = null;
        StartingPoint.IsSelected = false;
        StartingPoint = null;
    }

    public void TrackSegment(Transform toFollow)
    {
        if (_currentSegment is null)
        {
            return;
        }
        _currentSegment.LookAt(toFollow);
        _currentSegment.RotateAround(_currentSegment.position, _currentSegment.right, 90);
        _currentSegment.localScale = new Vector3(
            _currentSegment.localScale.x,
            Vector3.Distance(toFollow.position, _currentSegment.position) / 2,
            _currentSegment.localScale.z
        );
    }
}
