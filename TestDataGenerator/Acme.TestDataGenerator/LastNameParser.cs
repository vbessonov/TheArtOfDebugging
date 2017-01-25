using Acme.WebParser;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace Acme.TestDataGenerator
{
    internal class LastNameParser : Parser<LastNameInfo>
    {
        protected override IEnumerable<LastNameInfo> ParsePageContent(string pageContent)
        {
            var document = new HtmlDocument();

            document.LoadHtml(pageContent);

            var table = document.DocumentNode.SelectSingleNode(@"//table[2]//table");
            var rows = table.SelectNodes(@"tr");
            var names = new List<LastNameInfo>();

            foreach (var row in rows.Skip(2))
            {
                var rankNode = row.SelectSingleNode(@"td[1]");
                var rank = int.Parse(rankNode.InnerText.Replace(".", string.Empty));
                var nameNode = row.SelectSingleNode(@"td[2]");
                var name = nameNode.InnerText;

                names.Add(new LastNameInfo(name, rank));
            }

            return names;
        }
    }
}
