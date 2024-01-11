namespace StereoApp.Model
{
    public class PointManager
    {
        private char nextLabel = 'A';
        public string NextLabel => nextLabel.ToString();

        public string GenerateNextLabel()
        {
            return (nextLabel++).ToString();
        }

        public Point Create(float x, float y, float z)
        {
            var point = new Point(x, y, z, GenerateNextLabel());
            AppManager.Instance.points.Add(point);
            return point;
        }

        public Point Label(Point point)
        {
            point.Label = GenerateNextLabel();
            AppManager.Instance.points.Add(point);
            return point;
        }
    }
}
