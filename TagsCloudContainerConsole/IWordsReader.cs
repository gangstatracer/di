using System.Collections.Generic;
using System.IO;

namespace TagsCloudContainerConsole
{
    public interface IWordsReader
    {
        IEnumerable<string> GetWords(TextReader reader);
    }
}