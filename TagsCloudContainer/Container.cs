using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TagsCloudContainer
{
    public class Container
    {
        private readonly IWordsPreprocessor[] wordsPreprocessors;
        private readonly IWordsFilter[] wordsFilters;
        private readonly IWordsFramer wordsFramer;
        private readonly ICloudLayouter cloudLayouter;
        private readonly IWordsBitmapWriter bitmapWriter;

        public Container(IWordsPreprocessor[] wordsPreprocessors, IWordsFilter[] wordsFilters, IWordsFramer wordsFramer, ICloudLayouter cloudLayouter, IWordsBitmapWriter bitmapWriter)
        {
            this.wordsPreprocessors = wordsPreprocessors;
            this.wordsFilters = wordsFilters;
            this.wordsFramer = wordsFramer;
            this.cloudLayouter = cloudLayouter;
            this.bitmapWriter = bitmapWriter;
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

            var layoutedWords = frames.Select(frame => Tuple.Create(frame.Item1, cloudLayouter.PutNextRectangle(frame.Item2))).ToList();

            var result = new MemoryStream();
            bitmapWriter.Write(layoutedWords).Save(result, ImageFormat.Bmp);
            result.Seek(0, SeekOrigin.Begin);
            return result;
        }
    }
}