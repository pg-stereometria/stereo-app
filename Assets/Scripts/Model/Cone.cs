using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StereoApp.Model.Interfaces;

namespace StereoApp.Model
{
    public class Cone : SolidFigure, IConicalFrustum, ISerializableTo<Cone, SerializedCone>
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

        public override SerializedSolidFigure ToSerializableFigure()
        {
            return ToSerializable();
        }

        public SerializedCone ToSerializable()
        {
            return new SerializedCone { bottomBase = BottomBase.ToSerializable(), height = Height };
        }

        public override float TotalArea()
        {
            return MathF.PI
                * BottomBase.Radius
                * (
                    BottomBase.Radius
                    + MathF.Sqrt(MathF.Pow(BottomBase.Radius, 2) + MathF.Pow(Height, 2))
                );
        }

        public override float Volume()
        {
            return MathF.PI * MathF.Pow(BottomBase.Radius, 2) * (Height / 3);
        }
    }

    [Serializable]
    public class SerializedCone : SerializedSolidFigure, ISerializableFrom<SerializedCone, Cone>
    {
        public SerializedCircle bottomBase;
        public float height;

        public override SolidFigure ToActualFigure()
        {
            return ToActualType();
        }

        public Cone ToActualType()
        {
            return new Cone(bottomBase.ToActualType(), height);
        }
    }
}
