using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _xPositionInput;

    [SerializeField]
    private TMP_InputField _yPositionInput;

    [SerializeField]
    private TMP_InputField _zPositionInput;

    public static UIHandler Instance { get; private set; }

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

    public void CreatePointFromUI()
    {
        Vector3 position = new Vector3(
            float.Parse(_xPositionInput.text),
            float.Parse(_yPositionInput.text),
            float.Parse(_zPositionInput.text)
        );
        PointCreator.Instance.CreatePoint(position);
    }
}
