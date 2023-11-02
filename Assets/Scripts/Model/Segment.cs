namespace StereoApp.Model
{
    public class Segment // a.k.a. vertex
    {
        public Point First { get; set; }
        public Point Second { get; set; }

        public Segment(Point first, Point second)
        {
            First = first;
            Second = second;
        }
    }
}
