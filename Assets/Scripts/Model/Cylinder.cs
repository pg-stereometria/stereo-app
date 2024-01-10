using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StereoApp.Model.Interfaces;

namespace StereoApp.Model
{
    public class Cylinder
        : SolidFigure,
            IConicalFrustum,
            ISerializableTo<Cylinder, SerializedCylinder>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Circle TopBase => Base;
        public Circle BottomBase => Base;
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

        private Circle _base;
        public Circle Base
        {
            get => _base;
            set
            {
                _base = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TopBase));
                OnPropertyChanged(nameof(BottomBase));
            }
        }

        public Cylinder(Circle @base, float height)
        {
            Base = @base;
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

        public SerializedCylinder ToSerializable()
        {
            return new SerializedCylinder { @base = Base.ToSerializable(), height = Height };
        }

        public override float TotalArea()
        {
            return 2 * MathF.PI * BottomBase.Radius * Height + 2 * MathF.PI * MathF.Pow(BottomBase.Radius, 2);
        }

        public override float Volume()
        {
            return MathF.PI * MathF.Pow(BottomBase.Radius, 2) * Height;
        }
    }

    [Serializable]
    public class SerializedCylinder
        : SerializedSolidFigure,
            ISerializableFrom<SerializedCylinder, Cylinder>
    {
        public SerializedCircle @base;
        public float height;

        public override SolidFigure ToActualFigure()
        {
            return ToActualType();
        }

        public Cylinder ToActualType()
        {
            return new Cylinder(@base.ToActualType(), height);
        }
    }
}
