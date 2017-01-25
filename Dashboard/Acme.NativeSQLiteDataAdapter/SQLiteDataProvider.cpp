#include "stdafx.h"
#include "SQLiteDataProvider.h"
#include "SQLiteRepository.h"

using namespace System;
using namespace System::Data;

using namespace Acme::Data;
using namespace Acme::NativeSQLiteDataAdapter;

String^ SQLiteDataProvider::Name::get()
{
    return gcnew String("Native SQLite3 Data Provider");
}

bool SQLiteDataProvider::SupportsEncryption::get()
{
    return false;
}

IDataRepository^ SQLiteDataProvider::Open(System::String^ databaseFilePath)
{
    return gcnew SQLiteRepository(databaseFilePath);
}

IDataRepository^ SQLiteDataProvider::Open(System::String^ databaseFilePath, System::String^ password)
{
    throw gcnew NotSupportedException();
}

