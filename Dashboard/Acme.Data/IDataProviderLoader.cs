using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acme.Data
{
    public interface IDataProviderLoader : IDisposable
    {
        IEnumerable<IDataProvider> DataProviders { get; }

        void Load();

        Task LoadAsync();
    }
}
