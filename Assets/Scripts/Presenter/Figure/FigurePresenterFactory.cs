using System;
using StereoApp.Model;
using StereoApp.Model.Interfaces;
using StereoApp.Presenter.Base;
using UnityEngine;

namespace StereoApp.Presenter.Figure
{
    public class FigurePresenterFactory : MonoBehaviour
    {
        [SerializeField]
        private GameObject _polyhedronPrefab;

        [SerializeField]
        private GameObject _spherePrefab;

        [SerializeField]
        private GameObject _conicalFrustumPrefab;

        public FigurePresenter FromFigure(object figure)
        {
            switch (figure)
            {
                case Polyhedron polyhedron:
                {
                    var gameObj = Instantiate(_polyhedronPrefab.gameObject, transform);
                    var presenter = gameObj.GetComponent<PolyhedronPresenter>();
                    presenter.Figure = polyhedron;
                    return presenter;
                }
                case Sphere sphere:
                {
                    var gameObj = Instantiate(_spherePrefab.gameObject, transform);
                    var presenter = gameObj.GetComponent<SpherePresenter>();
                    presenter.Figure = sphere;
                    return presenter;
                }
                case IConicalFrustum conicalFrustum:
                {
                    var gameObj = Instantiate(_conicalFrustumPrefab.gameObject, transform);
                    var presenter = gameObj.GetComponent<ConicalFrustumPresenter>();
                    presenter.Figure = conicalFrustum;
                    return presenter;
                }
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
