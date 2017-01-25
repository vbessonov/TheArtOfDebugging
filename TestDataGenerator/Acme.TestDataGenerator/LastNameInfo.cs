using System;

namespace Acme.TestDataGenerator
{
    internal class LastNameInfo : NameInfo
    {
        public LastNameInfo(string name)
            : base(name)
        {

        }

        public LastNameInfo(string name, int rank)
            : base(name, rank)
        {

        }
    }
}
