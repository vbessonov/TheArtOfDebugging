using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Acme.Data
{
    internal class InternalDataProviderLoader : MarshalByRefObject
    {
        public IEnumerable<IDataProvider> Load()
        {
            var dataProviders = new List<IDataProvider>();
            var assemblyFiles = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.dll");

            foreach (var assemblyFile in assemblyFiles)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(assemblyFile);
                    var dataProviderInterfaceType = typeof(IDataProvider);
                    var dataProviderTypes = assembly.GetTypes().Where(type => !type.IsAbstract && dataProviderInterfaceType.IsAssignableFrom(type));

                    foreach (var dataProviderType in dataProviderTypes)
                    {
                        try
                        {
                            var dataProvider = (IDataProvider)Activator.CreateInstance(dataProviderType);

                            dataProviders.Add(dataProvider);
                        }
                        catch { }
                    }
                }
                catch { }
            }

            return dataProviders;
        }
    }
}
