using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    internal class TagsCloudBitmapWriter_Should
    {
        [Test]
        public void Write_OneRectangle()
        {
            var bitmap = TagsCloudBitmapWriter
                .Write(new List<Rectangle> {new Rectangle(0, 0, 100, 37)});
            bitmap.Width.Should().Be(100);
            bitmap.Height.Should().Be(37);
        }

        [Test]
        public void ThrowException_NoRectangles()
        {
            Action writeAction = () => TagsCloudBitmapWriter.Write(new List<Rectangle>());
            writeAction.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void ShiftToPositiveCoordinates()
        {
            var bitmap = TagsCloudBitmapWriter
                .Write(new List<Rectangle>
                {
                    new Rectangle(-100, -50, 10, 20),
                    new Rectangle(43, 70, 5, 6)
                });
            bitmap.Width.Should().Be(148);
            bitmap.Height.Should().Be(126);
        }

        [Test]
        public void HasNoEffectOnInitialCollection()
        {
            var rectangles = new List<Rectangle>
            {
                new Rectangle(-100, -50, 10, 20),
                new Rectangle(43, 70, 5, 6)
            };
            TagsCloudBitmapWriter.Write(rectangles);
            rectangles.Should().Equal(rectangles);
        }
    }
}
