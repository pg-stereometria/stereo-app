using System.ComponentModel;
using StereoApp.Model;
using StereoApp.Presenter.Base;
using UnityEngine;

namespace StereoApp.Presenter.Figure
{
    public class PointPresenter : FigurePresenter<Point>
    {
        [SerializeField]
        private DisplayAboveObject displayAboveObject;

        public override Point Figure
        {
            set
            {
                var oldFigure = base.Figure;
                if (oldFigure != null)
                {
                    oldFigure.PropertyChanged -= OnPointPropertyChanged;
                }

                if (value != null)
                {
                    value.PropertyChanged += OnPointPropertyChanged;
                }

                if (displayAboveObject != null)
                {
                    displayAboveObject.Text = value?.Label;
                }

                base.Figure = value;
            }
        }

        private void OnPointPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (displayAboveObject != null)
            {
                displayAboveObject.Text = Figure.Label;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Destroy(displayAboveObject.gameObject);
        }
    }
}
