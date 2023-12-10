using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StereoApp.Model.Interfaces;

namespace StereoApp.Model
{
    public class Circle : ISerializableTo<Circle, SerializedCircle>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private float _radius;
        public float Radius
        {
            get => _radius;
            set
            {
                _radius = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Valid));
            }
        }
        public bool Valid => Radius > 0.0f;

        public static Circle Zero => new(0.0f);

        public Circle(float radius)
        {
            Radius = radius;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public SerializedCircle ToSerializable()
        {
            return new SerializedCircle { radius = Radius };
        }
    }

    [Serializable]
    public class SerializedCircle : ISerializableFrom<SerializedCircle, Circle>
    {
        public float radius;

        public Circle ToActualType()
        {
            return new Circle(radius);
        }
    }
}
