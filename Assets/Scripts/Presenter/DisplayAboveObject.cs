using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace StereoApp.Presenter
{
    public class DisplayAboveObject : MonoBehaviour
    {
        [SerializeField]
        private GameObject target;

        [SerializeField]
        private Vector3 offset;

        private Transform worldSpaceCanvas;
        private Transform cameraTransform;

        public string Text { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            worldSpaceCanvas = GameObject.FindWithTag("WorldSpaceCanvas").transform;
            transform.SetParent(worldSpaceCanvas);
            GetComponent<TMP_Text>().text = Text;

            cameraTransform = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
            transform.rotation = Quaternion.LookRotation(
                transform.position - cameraTransform.position,
                cameraTransform.up
            );
            transform.position = target.transform.position + offset;
            if (
                Physics.Raycast(
                    cameraTransform.position,
                    transform.position - cameraTransform.position
                )
            )
            {
                transform.position = target.transform.position - offset;
            }
        }
    }
}
