using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Acme.WebParser
{
    public abstract class Parser<T> : IParser<T>
    {
        private async Task<string> DownloadPageContent(string requestUri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(requestUri);

                using (var responseContent = response.Content)
                {
                    return await responseContent.ReadAsStringAsync();
                }
            }
        }

        protected abstract IEnumerable<T> ParsePageContent(string pageContent);

        public IEnumerable<T> Parse(string uri)
        {
            return ParseAsync(uri).Result;
        }

        public async Task<IEnumerable<T>> ParseAsync(string uri)
        {
            var pageContent = await DownloadPageContent(uri);

            return ParsePageContent(pageContent);
        }
    }
}
