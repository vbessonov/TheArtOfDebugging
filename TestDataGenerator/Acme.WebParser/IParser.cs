using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acme.WebParser
{
    public interface IParser<T>
    {
        IEnumerable<T> Parse(string uri);

        Task<IEnumerable<T>> ParseAsync(string uri);
    }
}
