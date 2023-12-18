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
                    oldFigure.First.segments.Remove(oldFigure);
                    oldFigure.Second.segments.Remove(oldFigure);
                }

                if (value != null)
                {
                    value.PropertyChanged += OnSegmentPropertyChanged;
                    value.First.segments.Add(value);
                    value.Second.segments.Add(value);
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
