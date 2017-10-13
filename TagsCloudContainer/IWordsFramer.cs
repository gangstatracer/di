using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainer
{
    public interface IWordsHeighter
    {
        IEnumerable<Tuple<string, int>> GetWithHeights(IEnumerable<string> words);
    }
}