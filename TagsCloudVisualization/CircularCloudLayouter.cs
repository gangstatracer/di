using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudContainer;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private readonly Point center;

        public IList<Rectangle> Rectangles { get; } = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle rectangle;
            if (!Rectangles.Any())
            {
                rectangle = new Rectangle(center, rectangleSize);
                rectangle.Offset(-1 * rectangle.Width / 2, -1 * rectangle.Height / 2);
            }
            else
            {
                var hull = ConvexHull.GetSides(Rectangles.SelectMany(r => r.GetVertices())).ToList();
                var closestSide = hull.First(h => h.DistanceTo(center) == hull.Min(hh => hh.DistanceTo(center)));
                rectangle = ClingToSegment(closestSide, rectangleSize);
            }
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle ClingToSegment(Segment segment, Size rectangleSize)
        {
            Point location;
            if (segment.IsVertical)
            {
                location = ClingToVerticalSegment(segment, rectangleSize);
            }
            else
            {
                if (segment.IsHorizontal)
                {
                    location = ClingToHorizontalSegment(segment, rectangleSize);
                }
                else
                {
                    location = ClingToDiagonalSegment(segment, rectangleSize);
                }
            }
            return new Rectangle(location, rectangleSize);
        }

        private Point ClingToDiagonalSegment(Segment segment, Size rectangleSize)
        {
            var location = segment.Middle;
            segment.OrderFromLeftToRight();
            var pseudoscalar = Positioner.Compare(center, segment);
            if (pseudoscalar == 0)
            {
                throw new Exception(
                    $"Hull should not contain center. Segment: ({segment.Item1};{segment.Item2}), center: {center}.");
            }
            if (segment.Item2.Y - segment.Item1.Y < 0)
            {
                if (pseudoscalar > 0)
                {
                    location.X -= rectangleSize.Width;
                    location.Y -= rectangleSize.Height;
                }
            }
            else
            {
                if (pseudoscalar > 0)
                {
                    location.Y -= rectangleSize.Height;
                }
                else
                {
                    location.X -= rectangleSize.Width;
                }
            }
            return location;
        }

        private Point ClingToHorizontalSegment(Segment segment, Size rectangleSize)
        {
            var location = new Point(
                Math.Abs(segment.Item1.X - segment.Item2.X) / 2 + Math.Min(segment.Item1.X, segment.Item2.X) -
                rectangleSize.Width / 2, segment.Item1.Y);
            if (segment.Item1.Y < center.Y)
            {
                location.Y -= rectangleSize.Height;
            }
            return location;
        }

        private Point ClingToVerticalSegment(Segment segment, Size rectangleSize)
        {
            var location = new Point(segment.Item1.X,
                Math.Abs(segment.Item1.Y - segment.Item2.Y) / 2 + Math.Min(segment.Item1.Y, segment.Item2.Y) -
                rectangleSize.Height / 2);
            if (segment.Item1.X < center.X)
            {
                location.X -= rectangleSize.Width;
            }
            return location;
        }
    }
}
