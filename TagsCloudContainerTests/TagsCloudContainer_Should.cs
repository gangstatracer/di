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
        private IWordsPreprocessor wordsPreprocessor;
        [SetUp]
        public void SetUp()
        {
            wordsPreprocessor = A.Fake<IWordsPreprocessor>();
            container = new Container(wordsPreprocessor);
        }

        [Test]
        public void ThrowArgumentException_WhenNoWordsProvided()
        {
            Action callWithNoWords = () => container.GetTagsCloud(new List<string>());
            callWithNoWords.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void CallPreprocessor()
        {
            var words = new[] {"a"};
            container.GetTagsCloud(words);
            A.CallTo(() => wordsPreprocessor.Process(null)).WithAnyArguments().MustHaveHappened();
        }
    }
}