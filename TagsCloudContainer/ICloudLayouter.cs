using System.Drawing;

namespace TagsCloudContainer
{
    public interface ICloudLayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}