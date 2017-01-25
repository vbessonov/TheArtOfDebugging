using System;
using System.Data;

namespace Acme.Data
{
    public class EncryptedDatabaseException : DataException
    {
        public EncryptedDatabaseException()
        {

        }

        public EncryptedDatabaseException(string message)
            : base(message)
        {

        }

        public EncryptedDatabaseException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
