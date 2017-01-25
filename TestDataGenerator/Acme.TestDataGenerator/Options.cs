using CommandLine;
using CommandLine.Text;

namespace Acme.TestDataGenerator
{
    internal class Options
    {
        [Option('c', "rows-count", Required = true, HelpText = "Count of rows to be generated.")]
        public int RowsCount { get; set; }

        [Option('o', "output-file", Required = true, HelpText = "Output file.")]
        public string OutputFile { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
