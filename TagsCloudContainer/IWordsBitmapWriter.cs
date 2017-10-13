using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainer
{
    public interface IWordsBitmapWriter
    {
        float GetWordWidth(string word, int height);
        Bitmap Write(IList<Tuple<string, Rectangle>> wordFrames);
    }
}