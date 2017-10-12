using System.Collections.Generic;

namespace TagsCloudContainer
{
    public interface IWordsFilter
    {
        IEnumerable<string> GetFiltered(IEnumerable<string> words);
    }
}