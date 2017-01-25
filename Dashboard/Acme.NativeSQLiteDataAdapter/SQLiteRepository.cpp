#include "stdafx.h"
#include "SQLiteRepository.h"

using namespace System;
using namespace System::Data;
using namespace System::Runtime::InteropServices;

using namespace Acme::Data;
using namespace Acme::NativeSQLiteDataAdapter;

int GetDataCallback(void* parameter, int columns, char** columnValues, char** columnNames)
{
    DataTable^ table = safe_cast<DataTable^>(GCHandle::FromIntPtr(IntPtr(parameter)).Target);
    DataRow^ row = table->NewRow();
    bool columnsExist = table->Columns->Count > 0;

    for (int i = 0; i < columns; i++)
    {
        if (!columnsExist)
        {
            table->Columns->Add(gcnew String(columnNames[i]));
        }

        row[gcnew String(columnNames[i])] = gcnew String(columnValues[i]);
    }

    table->Rows->Add(row);

    return 0;
}

String^ SQLiteRepository::Password::get()
{
    return nullptr;
}

void SQLiteRepository::Password::set(String^ value)
{
    throw gcnew NotSupportedException();
}

SQLiteRepository::SQLiteRepository(String^ databaseFilePath)
{
    IntPtr pDatabaseFilePath = Marshal::StringToHGlobalAnsi(databaseFilePath);
    pin_ptr<sqlite3*> pDbHandle = &_dbHandle;

    int result = sqlite3_open(static_cast<char*>(pDatabaseFilePath.ToPointer()), pDbHandle);

    if (result != SQLITE_OK)
    {
        throw gcnew DataException(String::Format("Cannot open a database. The result code is {0}", result));
    }

    CheckEncryption();
}

SQLiteRepository::SQLiteRepository(String^ databaseFilePath, String^ password)
{
    throw gcnew NotSupportedException();
}

SQLiteRepository::~SQLiteRepository()
{
    sqlite3_close(_dbHandle);
}

DataSet^ SQLiteRepository::GetData()
{
    DataSet^ dataset = gcnew DataSet();
    DataTable^ table = dataset->Tables->Add();
    
    ExecuteQuery("SELECT * FROM employees", table, GetDataCallback);

    return dataset;
}

void SQLiteRepository::CheckEncryption()
{
    ExecuteQuery("SELECT COUNT(*) FROM sqlite_master", nullptr, nullptr);
}

void SQLiteRepository::ExecuteQuery(System::String^ sql, System::Object^ parameter, DataCallback callback)
{
    char* errorMessage = nullptr;
    GCHandle parameterHandle = GCHandle::Alloc(parameter);
    IntPtr sqlStringPointer = Marshal::StringToHGlobalAnsi(sql);

    int result = sqlite3_exec(
        _dbHandle,
        static_cast<const char*>(sqlStringPointer.ToPointer()),
        callback,
        GCHandle::ToIntPtr(parameterHandle).ToPointer(),
        &errorMessage
    );

    if (errorMessage)
    {
        sqlite3_free(errorMessage);

        if (result == SQLITE_NOTADB)
        {
            throw gcnew EncryptedDatabaseException();
        }
        else
        {
            throw gcnew DataException(gcnew String(errorMessage));
        }
    }
}
