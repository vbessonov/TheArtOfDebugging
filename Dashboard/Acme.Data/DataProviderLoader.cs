using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acme.Data
{
    public class DataProviderLoader : IDataProviderLoader
    {
        private AppDomain _appDomain;

        private readonly List<IDataProvider> _dataProviders = new List<IDataProvider>();

        public IEnumerable<IDataProvider> DataProviders
        {
            get { return _dataProviders; }
        }

        private void Clear()
        {
            _dataProviders.Clear();

            if (_appDomain != null)
            {
                AppDomain.Unload(_appDomain);
            }
        }

        private InternalDataProviderLoader CreateLoaderInCurrentAppDomain()
        {
            var loader = new InternalDataProviderLoader();

            return loader;
        }

        private InternalDataProviderLoader CreateLoaderInSeparateAppDomain()
        {
            _appDomain = AppDomain.CreateDomain("Acme Data Provider Domain");

            var loaderType = typeof(InternalDataProviderLoader);
            var loader = (InternalDataProviderLoader)_appDomain.CreateInstanceFromAndUnwrap(loaderType.Assembly.Location, loaderType.FullName);

            return loader;
        }

        public void Load()
        {
            LoadAsync().Wait();
        }

        public Task LoadAsync()
        {
            return Task.Factory.StartNew(
                () =>
                {
                    Clear();

                    var loader = CreateLoaderInCurrentAppDomain();
                    var providers = loader.Load();

                    _dataProviders.AddRange(providers);
                }
            );
        }

        public void Dispose()
        {
            Clear();
        }
    }
}
