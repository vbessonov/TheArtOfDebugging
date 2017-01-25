using CommandLine;
using System;
using System.Linq;

namespace Acme.TestDataGenerator
{
    public static class Program
    {
        private static void GenerateTestData(Options options)
        {
            var firstNameParser = new FirstNameParser();
            var lastNameParser = new LastNameParser();
            var firstNames = firstNameParser.Parse("http://www.babynamewizard.com/the-top-1000-baby-names-of-2011-united-states-of-america").ToList();
            var lastNames = lastNameParser.Parse("http://surnames.behindthename.com/top/lists/united-states/1990").ToList();

            var generator = new SqlGenerator();

            generator.Generate(firstNames.ToList(), lastNames.ToList(), options.RowsCount, options.OutputFile);
        }

        public static void Main(string[] args)
        {
            var options = new Options();
            var isValid = Parser.Default.ParseArgumentsStrict(args, options);

            if (!isValid)
            {
                Console.WriteLine(options.GetUsage());
            }
            else
            {
                GenerateTestData(options);

                Console.WriteLine("Test script was successfully generated");
            }
        }
    }
}
