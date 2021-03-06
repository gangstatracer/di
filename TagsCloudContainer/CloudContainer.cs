using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TagsCloudContainer
{
    public class CloudContainer
    {
        private readonly IWordsPreprocessor[] wordsPreprocessors;
        private readonly IWordsFilter[] wordsFilters;
        private readonly IWordsHeighter wordsFramer;
        private readonly ICloudLayouter cloudLayouter;
        private readonly IWordsBitmapWriter bitmapWriter;
        private readonly ImageFormat imageFormat;

        public CloudContainer(
            IWordsPreprocessor[] wordsPreprocessors, 
            IWordsFilter[] wordsFilters, 
            IWordsHeighter wordsFramer, 
            ICloudLayouter cloudLayouter, 
            IWordsBitmapWriter bitmapWriter, 
            ImageFormat imageFormat)
        {
            this.wordsPreprocessors = wordsPreprocessors;
            this.wordsFilters = wordsFilters;
            this.wordsFramer = wordsFramer;
            this.cloudLayouter = cloudLayouter;
            this.bitmapWriter = bitmapWriter;
            this.imageFormat = imageFormat;
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

            var wordsWithHeights = wordsFramer.GetWithHeights(wordsList).OrderByDescending(wh => wh.Item2);
            var wordsWithSizes = wordsWithHeights.Select(wh => Tuple.Create(
                    wh.Item1,
                    new Size(
                        (int) Math.Round(bitmapWriter.GetWordWidth(wh.Item1, wh.Item2)), 
                        wh.Item2
                        )
                ));

            var layoutedWords = wordsWithSizes
                .Select(ws => Tuple.Create(ws.Item1, cloudLayouter.PutNextRectangle(ws.Item2))).ToList();

            var result = new MemoryStream();
            bitmapWriter.Write(layoutedWords).Save(result, imageFormat);
            result.Seek(0, SeekOrigin.Begin);
            return result;
        }
    }
}