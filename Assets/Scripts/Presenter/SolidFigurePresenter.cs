using System.Collections.Generic;
using System.Collections.Specialized;
using StereoApp.Model;
using UnityEngine;

namespace StereoApp.Presenter
{
    public class SolidFigurePresenter : MonoBehaviour
    {
        [SerializeField]
        private GameObject _polygonPrefab;

        private readonly List<GameObject> _polygons = new();

        private SolidFigure _solid = null;

        public SolidFigure Solid
        {
            get => _solid;
            set
            {
                if (_solid != null)
                {
                    value.CollectionChanged -= OnSolidCollectionChanged;
                }

                _solid = value;
                if (value != null)
                {
                    value.CollectionChanged += OnSolidCollectionChanged;
                }

                OnChange();
            }
        }

        public void Start()
        {
            OnChange();
        }

        private void OnSolidCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnChange();
        }

        private void OnChange()
        {
            if (_solid == null)
            {
                return;
            }

            CleanupPresenters();
            foreach (var polygon in _solid)
            {
                var polygonObj = Instantiate(_polygonPrefab, _polygonPrefab.transform.parent);
                polygonObj.GetComponent<PolygonPresenter>().Polygon = polygon;
                _polygons.Add(polygonObj);
            }
        }

        private void CleanupPresenters()
        {
            foreach (var gameObj in _polygons)
            {
                Destroy(gameObj);
            }

            _polygons.Clear();
        }

        public void OnDestroy()
        {
            CleanupPresenters();
        }
    }
}
