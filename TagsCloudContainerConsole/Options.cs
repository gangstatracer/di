using System;
using System.Drawing.Imaging;
using CommandLine;
using CommandLine.Text;

namespace TagsCloudContainerConsole
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input file name")]
        public string InputFile { get; set; }
        [Option('o',"output", DefaultValue = "tags_cloud", HelpText = "Output file name")]
        public string OutputFile { get; set; }
        [Option('f', "font-family", DefaultValue = "Arial", HelpText = "Tags font family")]
        public string FontFamily { get; set; }
        [Option("image-format", DefaultValue = SupportedImageFormats.Png, HelpText = "Output image file format")]
        public SupportedImageFormats ImageFormat { get; set; }
        [Option('w', "width", HelpText = "Output image width")]
        public int Width { get; set; }
        [Option('h', "height", HelpText = "Output image height")]
        public int Height { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this);
        }

        internal ImageFormat Format
        {
            get
            {
                switch (ImageFormat)
                {
                    case SupportedImageFormats.Bmp:
                        return System.Drawing.Imaging.ImageFormat.Bmp;
                    case SupportedImageFormats.Png:
                        return System.Drawing.Imaging.ImageFormat.Png;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        internal string OutPutFileName => $"{OutputFile}.{ImageFormat}";
    }
}