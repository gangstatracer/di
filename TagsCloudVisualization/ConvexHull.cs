using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class ConvexHull
    {
        public static IEnumerable<Segment> GetSides(IEnumerable<Point> points)
        {
            var unvisitedPoints = points.ToList();
            var start = unvisitedPoints.First(p => p.X == unvisitedPoints.Min(pp => pp.X));
            unvisitedPoints.Remove(start);
            unvisitedPoints.Add(start);
            var previous = start;
            Point next;
            do
            {
                next = unvisitedPoints.First();
                foreach (var point in unvisitedPoints.Skip(1))
                {
                    if(Positioner.Compare(point, new Segment(previous, next)) < 0)
                    {
                        next = point;
                    }
                }
                yield return new Segment(previous, next);
                previous = next;
                unvisitedPoints.Remove(next);
            } while (next != start);
        }
    }
}
