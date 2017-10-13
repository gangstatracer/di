using System;
using System.Collections.Generic;
using System.Drawing;
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

        internal string OutPutFileName => $"{OutputFile}.{ImageFormat.ToString().ToLower()}";

        internal List<string> BlackList { get; set; } = new List<string>();

        [Option('c',"color", DefaultValue = "Black", HelpText = "Words color")]
        public string ForegroundColor {
            get { return Foreground.Name; }
            set { Foreground = Color.FromName(value); }
        }
        internal Color Foreground { get; set; }

        [Option('b', "background-color", DefaultValue = "White", HelpText = "Background color")]
        public string BackgroundColor
        {
            get { return Background.Name; }
            set { Background = Color.FromName(value); }
        }
        internal Color Background { get; set; }
    }
}