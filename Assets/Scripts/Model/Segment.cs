using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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

        public Point First { get; }
        public Point Second { get; }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Segment([NotNull] Point first, [NotNull] Point second)
        {
            if (first.Location == second.Location)
            {
                throw new ArgumentException("Segment's points cannot be equal.");
            }
            First = first;
            Second = second;
        }

        public override int GetHashCode()
        {
            // Since First and Second can't be the same and order doesn't matter (per Equals()),
            // XOR is actually a decent way of combining these and simpler than
            // HashCode.Combine().
            return First.GetHashCode() ^ Second.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Segment);
        }

        public bool Equals(Segment other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.GetType() != other.GetType())
            {
                return false;
            }

            return (
                (First == other.First && Second == other.Second)
                || (Second == other.First && First == other.Second)
            );
        }

        public override string ToString()
        {
            return Label + "(" + First + "," + Second + ")";
        }

        public float GetLength()
        {
            return (First.ToVector3() - Second.ToVector3()).magnitude;
        }
    }
}
