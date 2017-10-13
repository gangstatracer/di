using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TagsCloudContainerConsole
{
    public class TextWordsReader : IWordsReader
    {
        public IEnumerable<string> GetWords(TextReader reader)
        {
            string line;
            do
            {
                line = reader.ReadLine() ?? string.Empty;
                foreach (var token in Regex.Split(line, "\\W"))
                {
                    yield return token;
                }
            } while (line != string.Empty);
        }
    }
}