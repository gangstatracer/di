using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;

namespace TagsCloudContainerTests
{
    public class FrequencyFramer_Should
    {
        private IList<string> defaultWords = new List<string> {"cat", "functionality", "internet", "cat", "cat"};
        [Test]
        public void ReturnEqualHeightSizes_WhenZeroStep()
        {
            var minHeight = 10;
            var framer = new FrequencyFramer(minHeight, 0);
            framer.BuildFrames(defaultWords).All(t => t.Item2.Height == minHeight).Should().BeTrue();
        }
    }
}