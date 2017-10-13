using System.Drawing;

namespace TagsCloudContainer
{
    public interface IWordsColorGenerator
    {
        Color GetColor(string word);
    }
}