using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Entities
{
    public class Block
    {
        public int Id {get;set;}
        public int MaxNodes { get; set; }
        public int NumNodes { get; set; }
        public int BlockSize { get; set; }
        public byte[] Address { get; set; }
        public Node Next { get; set; }
        public Block Child { get; set; }
    }
}
