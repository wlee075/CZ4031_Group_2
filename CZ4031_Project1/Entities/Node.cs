using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Entities
{
    public class Node
    {
        public int Level { get; set; }
        public int Bucket { get; set; }
        public string Key { get; set; }
        public string Pointer { get; set; }

    }
}
