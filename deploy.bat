RD /S /Q Test
C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe Dashboard\Dashboard.sln /t:Rebuild /p:Confuguration=Debug
xcopy /E /Y /EXCLUDE:ignore Dashboard\Acme.Dashboard\bin\Debug Test\

