using Acme.Data;
using System;

namespace Acme.ManagedSQLiteDataAdapter
{
    public class SQLiteDataProvider : DataProvider
    {
        public override string Name
        {
            get { return "Managed SQLite 3 Data Provider"; }
        }

        public override bool SupportsEncryption
        {
            get { return true; }
        }

        public override IDataRepository Open(string databaseFilePath)
        {
            return new SQLiteRepository(databaseFilePath);
        }

        public override IDataRepository Open(string databaseFilePath, string password)
        {
            return new SQLiteRepository(databaseFilePath, password);
        }
    }
}
