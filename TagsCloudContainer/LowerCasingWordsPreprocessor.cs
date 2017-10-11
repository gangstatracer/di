using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer
{
    public class LowerCasingWordsPreprocessor : IWordsPreprocessor
    {
        public IEnumerable<string> Process(IEnumerable<string> words)
        {
            return words.Select(word => word.ToLower());
        }
    }
}