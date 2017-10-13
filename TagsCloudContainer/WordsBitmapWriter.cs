using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudContainer
{
    public class WordsBitmapWriter : IWordsBitmapWriter
    {
        private readonly IWordsColorGenerator colorGenerator;
        private readonly string fontFamily;
        private readonly Color backgroundColor;
        private readonly Size imageSize;

        public WordsBitmapWriter(IWordsColorGenerator colorGenerator, string fontFamily, Color backgroundColor, Size imageSize)
        {
            this.colorGenerator = colorGenerator;
            this.fontFamily = fontFamily;
            this.backgroundColor = backgroundColor;
            this.imageSize = imageSize;
        }

        private Tuple<int, SizeF> AdjustFontSizeTo(string word, Func<SizeF, bool> sizeRestriction)
        {
            var bitmap = new Bitmap(1, 1);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                var fontSize = 0;
                SizeF stringSize;
                do
                {
                    fontSize++;
                    stringSize = graphics.MeasureString(word, new Font(fontFamily, fontSize));
                } while (sizeRestriction(stringSize));
                return Tuple.Create(fontSize - 1, graphics.MeasureString(word, new Font(fontFamily, fontSize - 1)));
            }
        }

        public float GetWordWidth(string word, int height)
        {
            return AdjustFontSizeTo(word, size => size.Height <= height).Item2.Width;
        }

        public Bitmap Write(IList<Tuple<string, Rectangle>> wordFrames)
        {
            var rectangles = wordFrames.Select(f => f.Item2).ToList();
            var offset = new Point(-1 * rectangles.Min(r => r.Left), -1 * rectangles.Min(r => r.Top));
            var shifted = wordFrames.Select(f => Tuple.Create(f.Item1, new Rectangle(f.Item2.X + offset.X, f.Item2.Y + offset.Y, f.Item2.Width, f.Item2.Height))).ToList();

            var bottom = shifted.Max(f => f.Item2.Bottom);
            var right = shifted.Max(f => f.Item2.Right);
            var bitmap = new Bitmap(right, bottom);
            using (var drawing = Graphics.FromImage(bitmap))
            {
                drawing.FillRectangle(new SolidBrush(backgroundColor),
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                foreach (var frame in shifted)
                {
                    var fontSize = AdjustFontSizeTo(frame.Item1,
                            stringSize => stringSize.Height <= frame.Item2.Height &&
                                          stringSize.Width <= frame.Item2.Width)
                        .Item1;
                    drawing.DrawString(frame.Item1, new Font(fontFamily, fontSize),
                        new SolidBrush(colorGenerator.GetColor(frame.Item1)), frame.Item2);
                }
                return imageSize == Size.Empty ? bitmap : Resize(bitmap, imageSize);
            }
        }

        public static Bitmap Resize(Image image, Size destSize)
        {
            var destRect = new Rectangle(new Point(0,0), destSize);
            var result = new Bitmap(destSize.Width, destSize.Height);
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return result;
        }
    }
}