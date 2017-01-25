using System;
using System.Data;

namespace Acme.Data
{
    public abstract class DataRepository : MarshalByRefObject, IDataRepository
    {
        public abstract string Password { get; set; }

        public abstract DataSet GetData();

        public abstract void Dispose();
    }
}
