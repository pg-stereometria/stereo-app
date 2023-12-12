using System.Collections.Specialized;
using StereoApp.Model;
using StereoApp.Presenter.Base;
using UnityEngine;

namespace StereoApp.Presenter.Figure
{
    public class PolyhedronPresenter : FigurePresenter<Polyhedron>
    {
        [SerializeField]
        private GameObject _polygonPrefab;

        public override Polyhedron Figure
        {
            set
            {
                var oldFigure = base.Figure;
                if (oldFigure != null)
                {
                    oldFigure.Faces.CollectionChanged -= OnFigureFacesCollectionChanged;
                }

                if (value != null)
                {
                    value.Faces.CollectionChanged += OnFigureFacesCollectionChanged;
                }

                base.Figure = value;
            }
        }

        private void OnFigureFacesCollectionChanged(
            object sender,
            NotifyCollectionChangedEventArgs e
        )
        {
            OnChange();
        }

        protected override void OnChange()
        {
            base.OnChange();

            var figure = Figure;
            if (figure == null)
            {
                return;
            }

            foreach (var polygon in figure.Faces)
            {
                var polygonObj = Instantiate(_polygonPrefab, transform.parent);
                polygonObj.GetComponent<PolygonPresenter>().Figure = polygon;
                TrackGameObject(polygonObj);
            }
        }
    }
}
