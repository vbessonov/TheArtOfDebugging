// Acme.NativeSQLiteDataAdapter.h

#pragma once

using namespace System;

namespace Acme { namespace NativeSQLiteDataAdapter {

    public ref class SQLiteDataProvider : public Acme::Data::DataProvider
    {
    public:
        virtual property System::String^ Name
        {
            virtual System::String^ get() override;
        }

        virtual property bool SupportsEncryption
        {
            virtual bool get() override;
        }

    public:
        virtual Acme::Data::IDataRepository^ Open(System::String^ databaseFilePath) override;

        virtual Acme::Data::IDataRepository^ Open(System::String^ databaseFilePath, System::String^ password) override;
    };
}}
