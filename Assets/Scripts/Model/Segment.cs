using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StereoApp.Model
{
    public class Segment : IEquatable<Segment>, INotifyPropertyChanged // a.k.a. vertex
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _label = "";
        public string Label
        {
            get => _label;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                _label = value;
                OnPropertyChanged();
            }
        }

        public Point First { get; set; }
        public Point Second { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Segment(Point first, Point second)
        {
            First = first;
            Second = second;
        }

        public bool Equals(Segment other)
        {
            if (
                (this.First == other.First && this.Second == other.Second)
                || (this.Second == other.First && this.First == other.Second)
            )
                return true;
            return false;
        }
    }
}
