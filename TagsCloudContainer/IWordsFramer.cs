using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainer
{
    public interface IWordsFramer
    {
        IEnumerable<Tuple<string, Size>> BuildFrames(IEnumerable<string> words);
    }
}