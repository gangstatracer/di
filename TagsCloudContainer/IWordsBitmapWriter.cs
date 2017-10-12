using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainer
{
    public interface IWordsBitmapWriter
    {
        Bitmap Write(IList<Tuple<string, Rectangle>> wordFrames);
    }
}