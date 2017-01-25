using Acme.Data;
using System;
using System.Data;
using System.Data.SQLite;

namespace Acme.ManagedSQLiteDataAdapter
{
    internal class SQLiteRepository : DataRepository
    {
        private const string DefaultPassword = "ItIsHardToCreateAGoodPassword";

        private readonly SQLiteConnection _connection;

        private string _password;

        public override string Password
        {
            get { return _password; }
            set
            {
                _connection.ChangePassword(value);
                _password = value;
            }
        }

        public SQLiteRepository(string databaseFilePath)
        {
            _connection = CreateConnection(databaseFilePath, null);

            if (!CheckEncryption(false))
            {
                Password = DefaultPassword;
            }
            else
            {
                _connection = CreateConnection(databaseFilePath, DefaultPassword);
                _password = DefaultPassword;

                CheckEncryption();
            }
        }

        public SQLiteRepository(string databaseFilePath, string password)
        {
            _connection = CreateConnection(databaseFilePath, password);
            _password = password;

            CheckEncryption();
        }

        private SQLiteConnection CreateConnection(string databaseFilePath, string password)
        {
            var connectionString = string.Format("DataSource={0};Version=3;", databaseFilePath);
            var connection = new SQLiteConnection(connectionString);

            if (!string.IsNullOrEmpty(password))
            {
                connection.SetPassword(password);
            }

            connection.Open();

            return connection;
        }

        private SQLiteCommand CreateCommand(SQLiteConnection connection, string text)
        {
            var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = text;

            return command;
        }

        private bool CheckEncryption(bool throwException = true)
        {
            bool result = false;
            var selectCommand = CreateCommand(_connection, "SELECT COUNT(*) FROM sqlite_master");

            try
            {
                selectCommand.ExecuteScalar();
            }
            catch (SQLiteException)
            {
                result = true;

                if (throwException)
                {
                    throw new EncryptedDatabaseException();
                }
            }
            finally
            {
                selectCommand.Dispose();
            }

            return result;
        }

        public override DataSet GetData()
        {
            var dataSet = new DataSet();
            var selectCommand = CreateCommand(_connection, "SELECT * FROM employees");
            var adapter = new SQLiteDataAdapter(selectCommand);

            adapter.Fill(dataSet);

            return dataSet;
        }

        public override void Dispose()
        {
            _connection.Close();
        }
    }
}
