using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudContainer
{
    public class WordsBitmapWriter : IWordsBitmapWriter
    {
        public Bitmap Write(IList<Tuple<string, Rectangle>> wordFrames)
        {
            var rectangles = wordFrames.Select(f => f.Item2).ToList();
            var offset = new Point(-1 * rectangles.Min(r => r.Left), -1 * rectangles.Min(r => r.Top));
            var shifted = wordFrames.Select(f => Tuple.Create(f.Item1, new Rectangle(f.Item2.X + offset.X, f.Item2.Y + offset.Y, f.Item2.Width, f.Item2.Height))).ToList();

            var bottom = shifted.Max(f => f.Item2.Bottom);
            var right = shifted.Max(f => f.Item2.Right);
            var bitmap = new Bitmap(right, bottom);
            var drawing = Graphics.FromImage(bitmap);
            drawing.FillRectangle(Brushes.White, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
            var random = new Random();
            foreach (var frame in shifted)
            {
                var color = Color.FromArgb(random.Next(0, 200), random.Next(0, 200), random.Next(0, 200));
                drawing.DrawString(frame.Item1, new Font("Arial", 8), new SolidBrush(color), frame.Item2);
                drawing.DrawRectangle(Pens.Black, frame.Item2);
            }

            return bitmap;
        }
    }
}