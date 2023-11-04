using UnityEngine;

namespace StereoApp
{
    public class PointCreator : MonoBehaviour
    {
        [SerializeField]
        private GameObject _pointPrefab;

        [SerializeField]
        private Transform _pointsParent;
        public static PointCreator Instance { get; private set; }

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

        public Point CreatePoint(Vector3 positionToCreate)
        {
            return Instantiate(_pointPrefab, positionToCreate, Quaternion.identity, _pointsParent)
                .GetComponent<Point>();
        }
    }
}
