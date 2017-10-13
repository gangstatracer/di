using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer
{
    public class FrequencyHeighter: IWordsHeighter
    {
        private readonly int minHeight;
        private readonly int heightStep;

        public FrequencyHeighter(int minHeight, int heightStep)
        {
            this.minHeight = minHeight;
            this.heightStep = heightStep;
        }
        public IEnumerable<Tuple<string, int>> GetWithHeights(IEnumerable<string> words)
        {
            return words.GroupBy(w => w).Select(g => Tuple.Create(g.Key, minHeight + heightStep * (g.Count() - 1)));
        }
    }
}