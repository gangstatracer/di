using System;
using System.Data.SqlClient;
using System.IO;
using Autofac;
using TagsCloudContainer;
using TagsCloudVisualization;

namespace TagsCloudContainerConsole
{
    public class Program
    {
        private static IContainer _container;
        private static void ConfigurationRoot()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<BlackListFilter>().As<IWordsFilter>();
            builder.RegisterType<LowerCasingWordsPreprocessor>().As<IWordsPreprocessor>();
            builder.RegisterType<ConstantWordColorGenerator>().As<IWordsColorGenerator>();
            builder.Register(c => new FrequencyHeighter(10, 10)).As<IWordsHeighter>();
            builder.RegisterType<WordsBitmapWriter>().As<IWordsBitmapWriter>();
            builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
            builder.RegisterType<Container>();
            _container = builder.Build();

        }
        public static void Main(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                ConfigurationRoot();
                var tagsCloudContainer = _container.Resolve<Container>();
                var wordsReader = _container.Resolve<IWordsReader>();
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
