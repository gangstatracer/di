using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer
{
    public class BlackListFilter : IWordsFilter
    {
        private readonly HashSet<string> blackList;

        public BlackListFilter(IEnumerable<string> blackList)
        {
            this.blackList = new HashSet<string>(blackList);
        }
        public IEnumerable<string> GetFiltered(IEnumerable<string> words)
        {
            return words.Where(w => !blackList.Contains(w));
        }
    }
}