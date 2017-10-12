using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;

namespace TagsCloudContainerTests
{
    public class BlackListFilter_Should
    {
        private BlackListFilter filter;
        private IEnumerable<string> blackList;
        private readonly IList<string> blackListWords = new List<string> { "for", "to", "of", "in", "on", "about" };
        [SetUp]
        public void SetUp()
        {
            blackList = A.Fake<IEnumerable<string>>();
            A.CallTo(() => blackList.GetEnumerator()).Returns(blackListWords.GetEnumerator());
            filter = new BlackListFilter(blackList);
        }

        [Test]
        public void FilterOutAllWordsInBlackList()
        {
            var allowedWords = new List<string> {"dog", "cat"};
            filter.GetFiltered(allowedWords.Concat(blackList)).ShouldBeEquivalentTo(allowedWords);
        }

        [Test]
        public void FilterOutNoneWordsIfBlackListIsEmpty()
        {
            var words = new[] {"abc"};
            A.CallTo(() => blackList.GetEnumerator()).Returns(new List<string>().GetEnumerator());
            filter.GetFiltered(words).ShouldBeEquivalentTo(words);
        }
    }
}