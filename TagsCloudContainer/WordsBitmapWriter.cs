using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudContainer
{
    public class WordsBitmapWriter : IWordsBitmapWriter
    {
        private readonly IWordsColorGenerator colorGenerator;
        private readonly string fontFamily;
        private readonly Color backgroundColor;

        public WordsBitmapWriter(IWordsColorGenerator colorGenerator, string fontFamily, Color backgroundColor)
        {
            this.colorGenerator = colorGenerator;
            this.fontFamily = fontFamily;
            this.backgroundColor = backgroundColor;
        }
        public Bitmap Write(IList<Tuple<string, Rectangle>> wordFrames)
        {
            var rectangles = wordFrames.Select(f => f.Item2).ToList();
            var offset = new Point(-1 * rectangles.Min(r => r.Left), -1 * rectangles.Min(r => r.Top));
            var shifted = wordFrames.Select(f => Tuple.Create(f.Item1, new Rectangle(f.Item2.X + offset.X, f.Item2.Y + offset.Y, f.Item2.Width, f.Item2.Height))).ToList();

            var bottom = shifted.Max(f => f.Item2.Bottom);
            var right = shifted.Max(f => f.Item2.Right);
            var bitmap = new Bitmap(right, bottom);
            var drawing = Graphics.FromImage(bitmap);
            drawing.FillRectangle(new SolidBrush(backgroundColor), new Rectangle(0, 0, bitmap.Width, bitmap.Height));
            foreach (var frame in shifted)
            {
                var fontSize = 0;
                SizeF stringSize;
                do
                {
                    fontSize++;
                    stringSize = drawing.MeasureString(frame.Item1, new Font(fontFamily, fontSize));
                } while (stringSize.Height <= frame.Item2.Height && stringSize.Width <= frame.Item2.Width);
                drawing.DrawString(frame.Item1, new Font(fontFamily, fontSize - 1), new SolidBrush(colorGenerator.GetColor(frame.Item1)), frame.Item2);
                drawing.DrawRectangle(Pens.Black, frame.Item2);
                drawing.DrawImage(bitmap, frame.Item2.Location);
            }

            return bitmap;
        }
    }
}