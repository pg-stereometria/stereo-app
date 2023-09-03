using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Point : MonoBehaviour
{
    private Color _baseColor = Color.blue;
    private Color _mouseOverColor = Color.yellow;
    private Color _selectionColor = Color.red;

    private Renderer _renderer;

    private bool _isSelected = false;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            UpdateMaterialColor();
        }
    }
    private bool _isMouseOver = false;
    public bool IsMouseOver
    {
        get => _isMouseOver;
        private set
        {
            _isMouseOver = value;
            UpdateMaterialColor();
        }
    }

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        UpdateMaterialColor();
    }

    void OnMouseEnter()
    {
        if (EditSpaceController.Instance.CurrentMode == Mode.CONNECT_POINTS)
            SegmentCreator.Instance.TrackSegment(transform);
        EditSpaceController.Instance.CanCreatePoints = false;

        IsMouseOver = true;
    }

    void OnMouseExit()
    {
        EditSpaceController.Instance.CanCreatePoints = true;

        IsMouseOver = false;
    }

    void OnMouseOver()
    {
        if (
            Input.GetMouseButtonDown(0)
            && EditSpaceController.Instance.CurrentMode != Mode.CONNECT_POINTS
        )
        {
            SegmentCreator.Instance.StartCreatingSegment(this);
        }
        else if (
            Input.GetMouseButtonDown(0)
            && EditSpaceController.Instance.CurrentMode == Mode.CONNECT_POINTS
        )
        {
            SegmentCreator.Instance.StopCreatingSegment(this);
        }
        else if (
            Input.GetMouseButtonUp(0)
            && EditSpaceController.Instance.CurrentMode == Mode.CONNECT_POINTS
        )
        {
            SegmentCreator.Instance.StopCreatingSegment(this);
        }
    }

    private void UpdateMaterialColor()
    {
        var color = _baseColor;
        if (_isSelected)
        {
            color = _selectionColor;
        }
        else if (_isMouseOver)
        {
            color = _mouseOverColor;
        }
        _renderer.material.color = color;
    }
}
