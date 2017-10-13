using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class TagsCloudBitmapWriter
    {
        public static Bitmap Write(IList<Rectangle> rectangles)
        {
            if (!rectangles.Any())
            {
                throw new ArgumentException("rectangles list is empty");
            }

            var offset = new Point(-1 * rectangles.Min(r => r.Left), -1 * rectangles.Min(r => r.Top));
            var shiftedRectangles = rectangles.Select(r => new Rectangle(r.X + offset.X, r.Y + offset.Y, r.Width, r.Height)).ToList();

            var bottom = shiftedRectangles.Max(r => r.Bottom);
            var right = shiftedRectangles.Max(r => r.Right);
            var bitmap = new Bitmap(right, bottom);
            var drawing = Graphics.FromImage(bitmap);
            drawing.FillRectangle(Brushes.White, new Rectangle(0,0, bitmap.Width, bitmap.Height));
            var random = new Random();
            foreach (var rectangle in shiftedRectangles)
            {
                var color = Color.FromArgb(random.Next(0, 200), random.Next(0, 200), random.Next(0, 200));
                drawing.FillRectangle(new SolidBrush(color), rectangle);
            }

            return bitmap;
        }
    }
}
