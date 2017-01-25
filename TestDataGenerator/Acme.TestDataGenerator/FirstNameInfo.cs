using System;

namespace Acme.TestDataGenerator
{
    internal class FirstNameInfo : NameInfo
    {
        public FirstNameType Type { get; set; }

        public FirstNameInfo(string name, FirstNameType type)
            : base(name)
        {
            Type = type;
        }

        public FirstNameInfo(string name, FirstNameType type, int rank)
            : base(name, rank)
        {
            Type = type;
        }
    }
}
