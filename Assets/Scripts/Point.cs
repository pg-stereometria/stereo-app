using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    [SerializeField]
    private Color _highlightColor = Color.yellow;
    private Color _startColor;
    private InputHandler.Mode _startMode;


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
        _startMode = InputHandler.Instance.CurrentMode;
        InputHandler.Instance.CanCreatePoints = false;
        InputHandler.Instance.CurrentMode = InputHandler.Mode.NONE;
        _startColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = _highlightColor;
    }
    void OnMouseExit()
    {
        InputHandler.Instance.CanCreatePoints = true;
        if (InputHandler.Instance.CurrentMode == InputHandler.Mode.NONE)
            InputHandler.Instance.CurrentMode = _startMode;
        GetComponent<Renderer>().material.color = _startColor;
    }

    void OnMouseOver()
    {
        if (!Input.GetMouseButtonDown(0))
            return;
        Debug.Log("Creating Segment");
        if (_startMode != InputHandler.Mode.CONNECT_POINTS)
            InputHandler.Instance.StartCreatingSegment(this);
        else
            InputHandler.Instance.StopCreatingSegment(this);
    }
}
