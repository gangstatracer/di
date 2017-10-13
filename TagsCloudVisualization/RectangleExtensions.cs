using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static IEnumerable<Point> GetVertices(this Rectangle rectangle)
        {
            yield return rectangle.Location;
            yield return new Point(rectangle.Right, rectangle.Top);
            yield return new Point(rectangle.Right, rectangle.Bottom);
            yield return new Point(rectangle.Left, rectangle.Bottom);
        }
    }
}