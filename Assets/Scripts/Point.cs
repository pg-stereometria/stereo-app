using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Point : MonoBehaviour
{
    [SerializeField]
    private Color _highlightColor = Color.yellow;
    private Color _startColor;
    private BasicsCreator.Mode _startMode;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseEnter()
    {
        if (BasicsCreator.Instance.CurrentMode == BasicsCreator.Mode.CONNECT_POINTS)
            BasicsCreator.Instance.TrackSegment(transform);
        _startMode = BasicsCreator.Instance.CurrentMode;
        BasicsCreator.Instance.CanCreatePoints = false;
        BasicsCreator.Instance.CurrentMode = BasicsCreator.Mode.NONE;
        _startColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = _highlightColor;
    }
    void OnMouseExit()
    {
        BasicsCreator.Instance.CanCreatePoints = true;
        if (BasicsCreator.Instance.CurrentMode == BasicsCreator.Mode.NONE)
            BasicsCreator.Instance.CurrentMode = _startMode;
        GetComponent<Renderer>().material.color = _startColor;
    }

    void OnMouseOver()
    {
        if (!Input.GetMouseButtonDown(0) || EventSystem.current.IsPointerOverGameObject())
            return;
        if (_startMode != BasicsCreator.Mode.CONNECT_POINTS)
            BasicsCreator.Instance.StartCreatingSegment(this);
        else
            BasicsCreator.Instance.StopCreatingSegment(this);
    }
}
