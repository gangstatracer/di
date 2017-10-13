using System.Drawing;

namespace TagsCloudVisualization
{
    public class Positioner
    {
        public static int Compare(Point p, Segment s)
        {
            return (s.Item2.X - s.Item1.X) * (p.Y - s.Item2.Y) - (s.Item2.Y - s.Item1.Y) * (p.X - s.Item2.X);
        }
    }
}