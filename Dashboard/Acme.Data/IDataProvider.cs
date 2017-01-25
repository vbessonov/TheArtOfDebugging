using System;

namespace Acme.Data
{
    public interface IDataProvider
    {
        string Name { get; }

        bool SupportsEncryption { get; }

        IDataRepository Open(string databaseFilePath);

        IDataRepository Open(string databaseFilePath, string password);
    }
}
