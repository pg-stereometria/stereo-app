using System.ComponentModel;
using UnityEngine;
using StereoApp.Model;
using StereoApp.Presenter.Base;

namespace StereoApp.Presenter.Figure
{
    public class SegmentPresenter : FigurePresenter<Segment>
    {
        [SerializeField]
        private DisplayAboveObject displayAboveObject;

        public override Segment Figure
        {
            set
            {
                var oldFigure = base.Figure;
                if (oldFigure != null)
                {
                    oldFigure.PropertyChanged -= OnSegmentPropertyChanged;
                }

                if (value != null)
                {
                    value.PropertyChanged += OnSegmentPropertyChanged;
                }

                if (displayAboveObject != null)
                {
                    displayAboveObject.Text = value?.Label;
                }

                base.Figure = value;
            }
        }

        private void OnSegmentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (displayAboveObject != null)
            {
                displayAboveObject.Text = Figure.Label;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (displayAboveObject != null)
            {
                Destroy(displayAboveObject.gameObject);
            }
        }
    }
}
