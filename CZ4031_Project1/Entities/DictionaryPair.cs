using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Entities
{
    public class DictionaryPair: IComparable
    {
        public int key;
        public double value;

        public DictionaryPair(int key, double value)
        {
            this.key = key;
            this.value = value;
        }

        public int CompareTo(object obj)
        {
            DictionaryPair o = (DictionaryPair)obj;
            if (key == o.key)
            {
                return 0;
            }
            else if (key > o.key)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}
