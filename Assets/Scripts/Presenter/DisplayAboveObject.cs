using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace StereoApp.Presenter
{
    public class DisplayAboveObject : MonoBehaviour
    {
        public GameObject target;
        public Vector3 offset;
        public Vector3 forwardVector;

        [SerializeField]
        private bool onObject = true;

        [SerializeField]
        private float offsetValue = 0.2f;

        private Transform worldSpaceCanvas;
        private Transform cameraTransform;

        private string _text;
        private bool _fliped = false;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                GetComponent<TMP_Text>().text = Text;
            }
        }

        // Start is called before the first frame update
        void Awake()
        {
            worldSpaceCanvas = GameObject.FindWithTag("WorldSpaceCanvas").transform;
            transform.SetParent(worldSpaceCanvas);
            GetComponent<TMP_Text>().text = Text;

            cameraTransform = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (onObject)
            {
                var vectorToCamera = cameraTransform.position - target.transform.position;
                vectorToCamera.Normalize();
                offset = vectorToCamera * offsetValue;

                transform.position = target.transform.position + offset;
                transform.rotation = Quaternion.LookRotation(
                    transform.position - cameraTransform.position,
                    cameraTransform.up
                );
            }
            else
            {
                var val = Vector3.Dot(forwardVector, Camera.main.transform.position);
                if (val < 0 && _fliped)
                {
                    _fliped = false;
                    transform.localScale = new Vector3(
                        -transform.localScale.x,
                        transform.localScale.y,
                        transform.localScale.z
                    );
                }
                else if (val > 0 && !_fliped)
                {
                    _fliped = true;
                    transform.localScale = new Vector3(
                        -transform.localScale.x,
                        transform.localScale.y,
                        transform.localScale.z
                    );
                }

                transform.position = target.transform.position + offset;
                transform.rotation = Quaternion.LookRotation(forwardVector, cameraTransform.up);
            }
        }
    }
}
