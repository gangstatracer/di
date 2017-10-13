using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using Autofac;
using TagsCloudContainer;
using TagsCloudVisualization;

namespace TagsCloudContainerConsole
{
    public class Program
    {
        private static IContainer _container;
        private static void ConfigurationRoot(Options options)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TextWordsReader>().As<IWordsReader>();
            builder.Register(c => new BlackListFilter(options.BlackList)).As<IWordsFilter>();
            builder.RegisterType<LowerCasingWordsPreprocessor>().As<IWordsPreprocessor>();
            builder.Register(c => new ConstantWordColorGenerator(Color.DarkOrange)).As<IWordsColorGenerator>();
            builder.Register(c => new FrequencyHeighter(10, 10)).As<IWordsHeighter>();
            builder.Register(c => new WordsBitmapWriter(c.Resolve<IWordsColorGenerator>(), options.FontFamily, Color.DarkSeaGreen))
                .As<IWordsBitmapWriter>();
            builder.Register(c => new CircularCloudLayouter(new Point(100, 100))).As<ICloudLayouter>();
            builder.RegisterType<Container>().AsSelf();
            _container = builder.Build();

        }
        public static void Main(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                ConfigurationRoot(options);
                var wordsReader = _container.Resolve<IWordsReader>();
                using (var prepositionsDict = File.OpenText("boring.txt"))
                {
                    var prepositionsBlackList = wordsReader.GetWords(prepositionsDict);
                    options.BlackList.AddRange(prepositionsBlackList);
                }

                var tagsCloudContainer = _container.Resolve<Container>(new NamedParameter("imageFormat", options.Format));
                
                using (var input = File.OpenText(options.InputFile))
                {
                    var result = tagsCloudContainer.GetTagsCloud(wordsReader.GetWords(input));
                    using (var output = File.OpenWrite(options.OutPutFileName))
                    {
                        result.CopyTo(output);
                    }
                }
            }
        }
    }
}
