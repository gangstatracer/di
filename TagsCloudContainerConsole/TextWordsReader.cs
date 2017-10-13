using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TagsCloudContainerConsole
{
    public class TextWordsReader : IWordsReader
    {
        public IEnumerable<string> GetWords(TextReader reader)
        {
            var line = reader.ReadLine();
            while(line != null)
            {
                foreach (var token in Regex.Split(line, "\\W"))
                {
                    yield return token;
                }
                line = reader.ReadLine();
            }
        }
    }
}