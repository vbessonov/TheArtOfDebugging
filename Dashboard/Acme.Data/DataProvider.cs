using System;

namespace Acme.Data
{
    public abstract class DataProvider : MarshalByRefObject, IDataProvider
    {
        public abstract string Name { get; }

        public abstract bool SupportsEncryption { get; }

        public abstract IDataRepository Open(string databaseFilePath);

        public abstract IDataRepository Open(string databaseFilePath, string password);
    }
}
