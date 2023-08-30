using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Point : MonoBehaviour
{
    [SerializeField]
    private Color _highlightColor = Color.yellow;
    private Color _startColor;

    void OnMouseEnter()
    {
        if (BasicsCreator.Instance.CurrentMode == Mode.CONNECT_POINTS)
            SegmentCreator.Instance.TrackSegment(transform);
        BasicsCreator.Instance.CanCreatePoints = false;
    }

    void OnMouseExit()
    {
        BasicsCreator.Instance.CanCreatePoints = true;
    }

    void OnMouseOver()
    {
        if (
            Input.GetMouseButtonDown(0) && BasicsCreator.Instance.CurrentMode != Mode.CONNECT_POINTS
        )
        {
            SegmentCreator.Instance.StartCreatingSegment(this);
        }
        else if (
            Input.GetMouseButtonDown(0) && BasicsCreator.Instance.CurrentMode == Mode.CONNECT_POINTS
        )
        {
            SegmentCreator.Instance.StopCreatingSegment(this);
        }
        else if (
            Input.GetMouseButtonUp(0) && BasicsCreator.Instance.CurrentMode == Mode.CONNECT_POINTS
        )
        {
            SegmentCreator.Instance.StopCreatingSegment(this);
        }
    }

    public void HighlightPoint()
    {
        _startColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = _highlightColor;
    }

    public void ResetColor()
    {
        GetComponent<Renderer>().material.color = _startColor;
    }
}
