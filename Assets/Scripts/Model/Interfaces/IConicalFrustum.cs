using System.ComponentModel;

namespace StereoApp.Model.Interfaces
{
    public interface IConicalFrustum : INotifyPropertyChanged
    {
        public Circle TopBase { get; }
        public Circle BottomBase { get; }
        public float Height { get; }
    }
}
