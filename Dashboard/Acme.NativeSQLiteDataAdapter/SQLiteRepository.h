// Acme.NativeSQLiteDataAdapter.h
#include "sqlite3.h"

#pragma once

using namespace System;

namespace Acme { namespace NativeSQLiteDataAdapter {

    typedef int (*DataCallback)(void* parameter, int rows, char** columnValues, char** columnNames);

    ref class SQLiteRepository : public Acme::Data::DataRepository
    {
    public:
        virtual property System::String^ Password
        {
            virtual System::String^ get() override;

            virtual void set(System::String^ value) override;
        }

    public:
        SQLiteRepository(System::String^ databaseFilePath);

        SQLiteRepository(System::String^ databaseFilePath, System::String^ password);

        ~SQLiteRepository();

    public:
        virtual System::Data::DataSet^ GetData() override;

    private:
        void CheckEncryption();

        void ExecuteQuery(System::String^ sql, System::Object^ parameter, DataCallback callback);

    private:
        sqlite3* _dbHandle;
    };
}}
