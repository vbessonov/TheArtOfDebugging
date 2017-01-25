using System;

namespace Acme.TestDataGenerator
{
    internal abstract class NameInfo
    {
        private int _rank;

        private string _name;

        public int Rank
        {
            get { return _rank; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                _rank = value;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Name must be non-empty");
                }

                _name = value;
            }
        }

        protected NameInfo(string name)
        {
            Name = name;
        }

        protected NameInfo(string name, int rank)
        {
            Name = name;
            Rank = rank;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
