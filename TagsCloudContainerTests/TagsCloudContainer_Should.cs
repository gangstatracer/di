using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using FakeItEasy;
using NUnit.Framework;
using TagsCloudContainer;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudContainerTests
{
    public class TagsCloudContainer_Should
    {
        private Container container;
        private IWordsPreprocessor wordsPreprocessor1;
        private IWordsPreprocessor wordsPreprocessor2;
        private IWordsFilter wordsFilter1;
        private IWordsFilter wordsFilter2;
        private IWordsHeighter wordsFramer;
        private ICloudLayouter layouter;
        private IWordsBitmapWriter writer;
        private IList<string> defaultWords;
        private IList<int> defaultHeights;
        private readonly Bitmap bitmapResult = new Bitmap(1, 1); 
        [SetUp]
        public void SetUp()
        {
            defaultWords = new List<string> {"di", "solid", "mocking", "unit", "to"};
            defaultHeights = Enumerable.Repeat(30, defaultWords.Count).ToList();

            wordsPreprocessor1 = A.Fake<IWordsPreprocessor>();
            wordsPreprocessor2 = A.Fake<IWordsPreprocessor>();
            wordsFilter1 = A.Fake<IWordsFilter>();
            wordsFilter2 = A.Fake<IWordsFilter>();
            wordsFramer = A.Fake<IWordsHeighter>();
            layouter = A.Fake<ICloudLayouter>();
            writer = A.Fake<IWordsBitmapWriter>();

            A.CallTo(() => wordsPreprocessor1.Process(null)).WithAnyArguments().Returns(defaultWords);
            A.CallTo(() => wordsPreprocessor2.Process(null)).WithAnyArguments().Returns(defaultWords);
            A.CallTo(() => wordsFilter1.GetFiltered(null)).WithAnyArguments().Returns(defaultWords);
            A.CallTo(() => wordsFilter2.GetFiltered(null)).WithAnyArguments().Returns(defaultWords);
            A.CallTo(() => wordsFramer.GetWithHeights(null)).WithAnyArguments().Returns(defaultWords.Zip(defaultHeights, Tuple.Create));
            A.CallTo(() => layouter.PutNextRectangle(Size.Empty)).WithAnyArguments().Returns(Rectangle.Empty);
            A.CallTo(() => writer.Write(null)).WithAnyArguments().Returns(bitmapResult);

            container = new Container(
                new [] {wordsPreprocessor1, wordsPreprocessor2}, 
                new [] {wordsFilter1, wordsFilter2}, 
                wordsFramer, 
                layouter, 
                writer);
        }

        [Test]
        public void ThrowArgumentException_WhenNoWordsProvided()
        {
            Action callWithNoWords = () => container.GetTagsCloud(new List<string>());
            callWithNoWords.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void CallAllPreprocessors()
        {
            container.GetTagsCloud(defaultWords);
            A.CallTo(() => wordsPreprocessor1.Process(null)).WithAnyArguments().MustHaveHappened();
            A.CallTo(() => wordsPreprocessor2.Process(null)).WithAnyArguments().MustHaveHappened();
        }

        [Test]
        public void CallAllWordsFilters()
        {
            container.GetTagsCloud(defaultWords);
            A.CallTo(() => wordsFilter1.GetFiltered(null)).WithAnyArguments().MustHaveHappened();
            A.CallTo(() => wordsFilter2.GetFiltered(null)).WithAnyArguments().MustHaveHappened();
        }

        [Test]
        public void CallLayouter()
        {
            container.GetTagsCloud(defaultWords);
            A.CallTo(() => layouter.PutNextRectangle(new Size())).WithAnyArguments().MustHaveHappened();
        }

        [Test]
        public void IntegrateSuccessfullyWith_LowerCasingPreprocessor_BlackListFilter_FrequencyFramer_BitmapWriter()
        {
            container = new Container(
                new IWordsPreprocessor[]{new LowerCasingWordsPreprocessor()},
                new IWordsFilter[]{new BlackListFilter(new []{"to"}) },
                new FrequencyHeighter(50, 10),
                new CircularCloudLayouter(new Point(100, 100)),
                new WordsBitmapWriter(new ConstantWordColorGenerator(Color.Brown), "Arial", Color.AliceBlue));
            var stream = container.GetTagsCloud(defaultWords);
            using (var output = File.OpenWrite("result.bmp"))
            {
                stream.CopyTo(output);
            }
        }
    }
}