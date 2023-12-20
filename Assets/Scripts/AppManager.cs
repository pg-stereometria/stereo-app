using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StereoApp
{
    public class AppManager : MonoBehaviour
    {
        public static AppManager Instance { get; set; }

        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        public Model.SolidFigure figure;
        public Vector3 midpoint = new Vector3(0,0,0);
        public float longestDistance;
    }
}
