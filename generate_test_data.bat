IF NOT EXIST TestData mkdir TestData
C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe TestDataGenerator\TestDataGenerator.sln /t:Build /p:Configuration=Release
TestDataGenerator\Acme.TestDataGenerator\bin\Release\Acme.TestDataGenerator.exe --rows-count 1000000 --output-file TestData\employees.sql
rem sqlite3.exe TestData\employees.db3 < TestData\employees.sql
rem copy TestData\employees.db3 Test
