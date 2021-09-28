using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Entities
{
    public class BPlusTree
    {
        public Block rootBlock { get; set; }

        public int MaxKeys { get; set; }

        public static int Levels { get; set; }

        public int NumNodes { get; set; }

    }
}