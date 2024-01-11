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

        private Segment _segment;
        public override Segment Figure
        {
            set
            {
                var oldFigure = base.Figure;
                if (oldFigure != null)
                {
                    oldFigure.PropertyChanged -= OnSegmentPropertyChanged;
                    oldFigure.First.PropertyChanged -= OnPointPropertyChanged;
                    oldFigure.Second.PropertyChanged -= OnPointPropertyChanged;
                }

                if (value != null)
                {
                    value.PropertyChanged += OnSegmentPropertyChanged;
                    value.First.PropertyChanged += OnPointPropertyChanged;
                    value.Second.PropertyChanged += OnPointPropertyChanged;
                }

                if (displayAboveObject != null)
                {
                    displayAboveObject.Text = value?.Label;
                }

                base.Figure = value;
                _segment = value;
                Initialize();
            }
        }

        private void Initialize()
        {
            if (_segment is null)
                return;
            var firstVertex = _segment.First.ToPosition();
            var secondVertex = _segment.Second.ToPosition();

            transform.position = firstVertex;
            transform.LookAt(secondVertex);
            transform.RotateAround(transform.position, transform.right, 90);

            var localScale = transform.localScale;
            localScale.y = Vector3.Distance(secondVertex, firstVertex) / 2;
            transform.localScale = localScale;
        }

        private void OnSegmentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (displayAboveObject != null)
            {
                displayAboveObject.Text = Figure.Label;
            }
        }

        private void OnPointPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Initialize();
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
