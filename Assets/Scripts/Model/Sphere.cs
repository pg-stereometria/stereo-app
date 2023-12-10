using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StereoApp.Model.Interfaces;
using StereoApp.Utils;

namespace StereoApp.Model
{
    public class Sphere
        : SolidFigure,
            ISerializableTo<Sphere, SerializedSphere>,
            INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private PropertyChangedEventHandler _onCirclePropertyChanged;
        private Circle _circle;
        public Circle Circle
        {
            get => _circle;
            set
            {
                if (_onCirclePropertyChanged != null)
                {
                    _circle.PropertyChanged -= _onCirclePropertyChanged;
                }

                _onCirclePropertyChanged = WeakEventHandler.Create(
                    (PropertyChangedEventHandler)OnCirclePropertyChanged,
                    (handler) => value.PropertyChanged -= handler
                );
                value.PropertyChanged += _onCirclePropertyChanged;
                _circle = value;
                OnPropertyChanged(nameof(Radius));
            }
        }

        public float Radius => Circle.Radius;

        public Sphere(float radius)
            : this(new Circle(radius)) { }

        public Sphere(Circle circle)
        {
            Circle = circle;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override SerializedSolidFigure ToSerializableFigure()
        {
            return ToSerializable();
        }

        public SerializedSphere ToSerializable()
        {
            return new SerializedSphere { circle = Circle.ToSerializable() };
        }

        private void OnCirclePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Circle.Radius))
            {
                OnPropertyChanged(nameof(Radius));
            }
        }
    }

    [Serializable]
    public class SerializedSphere
        : SerializedSolidFigure,
            ISerializableFrom<SerializedSphere, Sphere>
    {
        public SerializedCircle circle;

        public override SolidFigure ToActualFigure()
        {
            return ToActualType();
        }

        public Sphere ToActualType()
        {
            return new Sphere(circle.ToActualType());
        }
    }
}
