using StereoApp.Model;
using StereoApp.Presenter.Base;
using UnityEngine;

namespace StereoApp.Presenter.Figure
{
    public class SolidFigurePresenter : FigurePresenter<SolidFigure>
    {
        [SerializeField]
        private FigurePresenterFactory _figurePresenterFactory;

        private FigurePresenter _actualPresenter;

        protected override void OnChange()
        {
            base.OnChange();
            if (_actualPresenter != null)
            {
                _actualPresenter.FigureObj = null;
                Destroy(_actualPresenter.gameObject);
                _actualPresenter = null;
            }

            var figure = Figure;
            if (figure != null)
            {
                _actualPresenter = _figurePresenterFactory.FromFigure(figure);
            }
        }
    }
}
