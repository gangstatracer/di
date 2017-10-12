using System;
using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using TagsCloudContainer;
using FluentAssertions;

namespace TagsCloudContainerTests
{
    public class TagsCloudContainer_Should
    {
        private Container container;
        private IWordsPreprocessor wordsPreprocessor1;
        private IWordsPreprocessor wordsPreprocessor2;
        private IWordsFilter wordsFilter1;
        private IWordsFilter wordsFilter2;
        private IWordsFramer wordsFramer;
        private readonly IList<string> defaultWords = new List<string> {"di", "solid", "mocking", "unit", "to"};
        [SetUp]
        public void SetUp()
        {
            wordsPreprocessor1 = A.Fake<IWordsPreprocessor>();
            wordsPreprocessor2 = A.Fake<IWordsPreprocessor>();
            wordsFilter1 = A.Fake<IWordsFilter>();
            wordsFilter2 = A.Fake<IWordsFilter>();
            wordsFramer = A.Fake<IWordsFramer>();
            container = new Container(new [] {wordsPreprocessor1, wordsPreprocessor2}, new [] {wordsFilter1, wordsFilter2}, wordsFramer);
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
    }
}