using System.Drawing;

namespace TagsCloudContainer
{
    public class ConstantWordColorGenerator : IWordsColorGenerator
    {
        private readonly Color color;

        public ConstantWordColorGenerator(Color color)
        {
            this.color = color;
        }
        public Color GetColor(string word)
        {
            return color;
        }
    }
}