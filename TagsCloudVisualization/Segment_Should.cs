using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class Segment_Should
    {
        [Test]
        public void HasZeroLengh_WhenStartEqualsEnd()
        {
            new Segment(new Point(1, 1), new Point(1, 1)).Length.Should().Be(0);
        }

        [Test]
        public void HaveProperMiddle_WhenNonZeroLength()
        {
            new Segment(new Point(1, 1), new Point(1, 3)).Middle.Should().Be(new Point(1, 2));
        }

        [Test]
        public void EqualsToSelf()
        {
            var s = new Segment(new Point(1, 1), new Point(3, 7));
            s.Should().Be(s);
        }

        [Test]
        public void HaveProperLength_WhenStartNotEqualsEnd()
        {
            new Segment(new Point(1, 1), new Point(1, 3)).Should().Be(4);
        }

        [Test]
        public void ReturnDistanceToPoint()
        {
            var s = new Segment(new Point(1, 1), new Point(1, 3));
            s.DistanceTo(new Point(5, 2)).Should().Be(2);
        }
    }
}