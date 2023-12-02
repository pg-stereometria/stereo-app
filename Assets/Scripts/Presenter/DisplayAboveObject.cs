using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace StereoApp.Presenter
{
    public class DisplayAboveObject : MonoBehaviour
    {
        [SerializeField]
        private PointPresenter pointPresenter;

        [SerializeField]
        private Vector3 offset;

        private Transform worldSpaceCanvas;
        private Transform cameraTransform;

        // Start is called before the first frame update
        void Start()
        {
            worldSpaceCanvas = GameObject.FindWithTag("WorldSpaceCanvas").transform;
            transform.SetParent(worldSpaceCanvas);
            GetComponent<TMP_Text>().text = pointPresenter.Point.Label;

            cameraTransform = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
            transform.rotation = Quaternion.LookRotation(
                transform.position - cameraTransform.position,
                cameraTransform.up
            );
            transform.position = pointPresenter.transform.position + offset;
            if (
                Physics.Raycast(
                    cameraTransform.position,
                    transform.position - cameraTransform.position
                )
            )
            {
                transform.position = pointPresenter.transform.position - offset;
            }
        }
    }
}
