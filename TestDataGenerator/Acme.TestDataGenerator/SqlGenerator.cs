using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Acme.TestDataGenerator
{
    internal class SqlGenerator
    {
        private static string[] Departments =
        {
            "Sales",
            "IT",
            "Management",
            "HR",
            "Accounting"
        };

        private string EscapeString(string value)
        {
            return value.Replace("'", @"''");
        }

        public void Generate(IList<FirstNameInfo> firstNames, IList<LastNameInfo> lastNames, int rows, string outputFileName)
        {
            var random = new Random();
            var buffer = new StringBuilder(1024);

            using (var outputStream = File.CreateText(outputFileName))
            {
                outputStream.WriteLine("DROP TABLE IF EXISTS employees;");
                outputStream.WriteLine("CREATE TABLE employees (id INTEGER NOT NULL PRIMARY KEY, first_name VARCHAR(255), last_name VARCHAR(255), age INTEGER NOT NULL, department VARCHAR(255) NOT NULL);");
                outputStream.WriteLine("INSERT INTO employees (first_name, last_name, age, department) VALUES ");

                for (int i = 1; i <= rows; i++)
                {
                    var firstName = EscapeString(firstNames[random.Next(firstNames.Count)].Name);
                    var lastName = EscapeString(lastNames[random.Next(lastNames.Count)].Name);
                    var age = random.Next(18, 55);
                    var department = Departments[random.Next(Departments.Length)];

                    buffer.AppendFormat("('{0}', '{1}', {2}, '{3}')", firstName, lastName, age, department);

                    if (i != rows)
                    {
                        buffer.Append(",");
                    }
                    else
                    {
                        buffer.Append(";");
                    }

                    outputStream.WriteLine(buffer);

                    buffer.Clear();
                }
            }
        }
    }
}
