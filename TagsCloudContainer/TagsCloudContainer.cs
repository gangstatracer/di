using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagsCloudContainer
{
    public class Container
    {
        private readonly IWordsPreprocessor[] wordsPreprocessors;
        private readonly IWordsFilter[] wordsFilters;
        private readonly IWordsFramer wordsFramer;

        public Container(IWordsPreprocessor[] wordsPreprocessors, IWordsFilter[] wordsFilters, IWordsFramer wordsFramer, ICloudLayouter cloudLayouter)
        {
            this.wordsPreprocessors = wordsPreprocessors;
            this.wordsFilters = wordsFilters;
            this.wordsFramer = wordsFramer;
        }
        public Stream GetTagsCloud(IEnumerable<string> words)
        {
            var wordsList = words.ToList();
            if (!wordsList.Any())
            {
                throw new ArgumentException("No words were provided");
            }

            IEnumerable<string> wordsEnumerable = wordsList;

            wordsList = wordsPreprocessors
                .Aggregate(wordsEnumerable, (current, preprocessor) => preprocessor.Process(current))
                .ToList();

            wordsEnumerable = wordsList;
            wordsList = wordsFilters
                .Aggregate(wordsEnumerable, (current, filter) => filter.GetFiltered(current)).ToList();

            var frames = wordsFramer.BuildFrames(wordsList);


            return new MemoryStream();
        }
    }
}