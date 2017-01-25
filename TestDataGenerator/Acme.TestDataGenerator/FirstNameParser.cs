using Acme.WebParser;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace Acme.TestDataGenerator
{
    internal class FirstNameParser : Parser<FirstNameInfo>
    {
        protected override IEnumerable<FirstNameInfo> ParsePageContent(string pageContent)
        {
            var document = new HtmlDocument();

            document.LoadHtml(pageContent);

            var table = document.DocumentNode.SelectSingleNode(@"//table[@class='tableizer-table']");
            var rows = table.SelectNodes(@"tr");
            var names = new List<FirstNameInfo>();

            foreach (var row in rows.Skip(1))
            {
                var femaleNameRankNode = row.SelectSingleNode(@"td[1]");
                var femaleNameRank = int.Parse(femaleNameRankNode.InnerText);
                var femaleNameNode = row.SelectSingleNode(@"td[2]");
                var femaleName = femaleNameNode.InnerText;
                var maleNameRankNode = row.SelectSingleNode(@"td[6]");
                var maleNameRank = int.Parse(maleNameRankNode.InnerText);
                var maleNameNode = row.SelectSingleNode(@"td[7]");
                var maleName = maleNameNode.InnerText;

                names.Add(new FirstNameInfo(femaleName, FirstNameType.Female, femaleNameRank));
                names.Add(new FirstNameInfo(maleName, FirstNameType.Male, maleNameRank));
            }

            return names;
        }
    }
}
