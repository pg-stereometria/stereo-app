using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StereoApp.Model.Interfaces;

namespace StereoApp.Model
{
    public class Cone : ISerializableTo<Cone, SerializedCone>, IConicalFrustum
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Circle TopBase { get; }
        private Circle _bottomBase;
        public Circle BottomBase
        {
            get => _bottomBase;
            set
            {
                _bottomBase = value ?? throw new ArgumentNullException();
                OnPropertyChanged();
            }
        }
        private float _height;
        public float Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }

        public Cone(Circle bottomBase, float height)
        {
            TopBase = Circle.Zero;
            BottomBase = bottomBase;
            Height = height;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public SerializedCone ToSerializable()
        {
            return new SerializedCone { bottomBase = BottomBase.ToSerializable(), height = Height };
        }
    }

    [Serializable]
    public class SerializedCone : ISerializableFrom<SerializedCone, Cone>
    {
        public SerializedCircle bottomBase;
        public float height;

        public Cone ToActualType()
        {
            return new Cone(bottomBase.ToActualType(), height);
        }
    }
}
