using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StereoApp.Model.Interfaces;

namespace StereoApp.Model
{
    public class TruncatedCone
        : ISerializableTo<TruncatedCone, SerializedTruncatedCone>,
            IConicalFrustum
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Circle _topBase;
        public Circle TopBase
        {
            get => _topBase;
            set
            {
                if (value?.Radius > BottomBase.Radius)
                {
                    throw new ArgumentException(
                        "Radius of the top base must be less than the radius of the bottom base."
                    );
                }

                _topBase = value ?? throw new ArgumentNullException();
                OnPropertyChanged();
            }
        }
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

        public TruncatedCone(Circle topBase, Circle bottomBase, float height)
        {
            BottomBase = bottomBase;
            TopBase = topBase;
            Height = height;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public SerializedTruncatedCone ToSerializable()
        {
            return new SerializedTruncatedCone
            {
                topBase = TopBase.ToSerializable(),
                bottomBase = BottomBase.ToSerializable(),
                height = Height
            };
        }
    }

    [Serializable]
    public class SerializedTruncatedCone : ISerializableFrom<SerializedTruncatedCone, TruncatedCone>
    {
        public SerializedCircle topBase;
        public SerializedCircle bottomBase;
        public float height;

        public TruncatedCone ToActualType()
        {
            return new TruncatedCone(topBase.ToActualType(), bottomBase.ToActualType(), height);
        }
    }
}
