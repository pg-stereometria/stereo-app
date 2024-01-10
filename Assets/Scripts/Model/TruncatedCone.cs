using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StereoApp.Model.Interfaces;

namespace StereoApp.Model
{
    public class TruncatedCone
        : SolidFigure,
            IConicalFrustum,
            ISerializableTo<TruncatedCone, SerializedTruncatedCone>
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

        public override SerializedSolidFigure ToSerializableFigure()
        {
            return ToSerializable();
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

        public override float TotalArea()
        {
            var sh = MathF.Sqrt(
                MathF.Pow(BottomBase.Radius - TopBase.Radius, 2) + MathF.Pow(Height, 2)
            );
            return MathF.PI * MathF.Pow(BottomBase.Radius, 2)
                + MathF.PI * MathF.Pow(TopBase.Radius, 2)
                + MathF.PI * (BottomBase.Radius + TopBase.Radius) * sh;
        }

        public override float Volume()
        {
            return MathF.PI
                * (Height / 3)
                * (
                    MathF.Pow(BottomBase.Radius, 2)
                    + BottomBase.Radius * TopBase.Radius
                    + MathF.Pow(TopBase.Radius, 2)
                );
        }
    }

    [Serializable]
    public class SerializedTruncatedCone
        : SerializedSolidFigure,
            ISerializableFrom<SerializedTruncatedCone, TruncatedCone>
    {
        public SerializedCircle topBase;
        public SerializedCircle bottomBase;
        public float height;

        public override SolidFigure ToActualFigure()
        {
            return ToActualType();
        }

        public TruncatedCone ToActualType()
        {
            return new TruncatedCone(topBase.ToActualType(), bottomBase.ToActualType(), height);
        }
    }
}
