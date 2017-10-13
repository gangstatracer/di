using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    internal class ConvexHull_Should
    {
        [Test]
        public void BuildZeroLengthSegment_OnOnePoint()
        {
            var a = new Point(3, 7);
            ConvexHull.GetSides(new[] {a})
                .Should()
                .Equal(new Segment(a, a));
        }
        [Test]
        public void BuildSegment_OnTwoPoints()
        {
            var a = new Point(10, 10);
            var b = new Point(1, 2);
            ConvexHull.GetSides(new[] { a, b })
                .Should()
                .Equal(new List<Segment>
                {
                    new Segment(b, a),
                    new Segment(a, b)
                });
        }
        [Test]
        public void BuildTriangle_OnThreePoints()
        {
            var a = new Point(10, 10);
            var b = new Point(1, 2);
            var c = new Point(-4, -7);
            ConvexHull.GetSides(new[] {a, b, c})
                .Should()
                .BeEquivalentTo(new List<Segment>
                {
                    new Segment(a, b),
                    new Segment(b, c),
                    new Segment(c, a)
                });
        }

        [Test]
        public void SkipInnerPoint_OnFourPoints()
        {
            var a = new Point(0, 0);
            var b = new Point(0, 4);
            var c = new Point(4, 0);
            var d = new Point(1, 1);
            ConvexHull.GetSides(new[] { a, b, c, d })
                .Should()
                .BeEquivalentTo(new List<Segment>
                {
                    new Segment(a, c),
                    new Segment(c, b),
                    new Segment(b, a)
                });
        }

        [Test]
        public void BuildSquare_OnFivePoints()
        {
            var a = new Point(0, 0);
            var b = new Point(0, 2);
            var c = new Point(2, 0);
            var d = new Point(2, 2);
            ConvexHull.GetSides(new[] { a, b, c, d, new Point(1, 1) })
                .Should()
                .BeEquivalentTo(new List<Segment>
                {
                    new Segment(a, b),
                    new Segment(b, d),
                    new Segment(d, c),
                    new Segment(c, a)
                });
        }
    }
}