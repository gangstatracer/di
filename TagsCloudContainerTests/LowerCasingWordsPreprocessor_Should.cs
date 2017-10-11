using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;

namespace TagsCloudContainerTests
{
    public class LowerCasingWordsPreprocessor_Should
    {
        private readonly LowerCasingWordsPreprocessor preprocessor = new LowerCasingWordsPreprocessor();
        [Test]
        public void LowerCase_WordsWithCapitals()
        {
            preprocessor.Process(new [] {"BaB"}).ShouldBeEquivalentTo(new [] {"bab"});
        }

        [Test]
        public void Process_AllPassedWords()
        {
            var wordsCount = 5;
            preprocessor.Process(Enumerable.Repeat("aK", wordsCount))
                .Count().Should().Be(wordsCount);
        }
    }
}