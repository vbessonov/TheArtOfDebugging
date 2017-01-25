using System;
using System.Data;

namespace Acme.Data
{
    public interface IDataRepository : IDisposable
    {
        string Password { get; set; }

        DataSet GetData();
    }
}
