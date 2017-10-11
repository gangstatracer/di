using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudContainer
{
    public class Container
    {
        private readonly IWordsPreprocessor wordsPreprocessor;

        public Container(IWordsPreprocessor wordsPreprocessor)
        {
            this.wordsPreprocessor = wordsPreprocessor;
        }
        public Stream GetTagsCloud(IEnumerable<string> words)
        {
            var wordsList = words.ToList();
            if (!wordsList.Any())
            {
                throw new ArgumentException("No words were provided");
            }
            wordsList = wordsPreprocessor.Process(wordsList).ToList();
            return new MemoryStream();
        }
    }
}