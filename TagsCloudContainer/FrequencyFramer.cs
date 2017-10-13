using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudContainer
{
    public class FrequencyFramer: IWordsFramer
    {
        private readonly int minHeight;
        private readonly int heightStep;

        public FrequencyFramer(int minHeight, int heightStep)
        {
            this.minHeight = minHeight;
            this.heightStep = heightStep;
        }
        public IEnumerable<Tuple<string, Size>> BuildFrames(IEnumerable<string> words)
        {
            return words.Select(w => Tuple.Create(w, new Size(w.Length * minHeight, minHeight)));
        }
    }
}