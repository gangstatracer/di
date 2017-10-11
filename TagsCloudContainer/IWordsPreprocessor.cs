﻿using System.Collections.Generic;

namespace TagsCloudContainer
{
    public interface IWordsPreprocessor
    {
        IEnumerable<string> Process(IEnumerable<string> words);
    }
}