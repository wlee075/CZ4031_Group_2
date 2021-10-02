using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Entities
{
    public class Block
    {
        public int maxNodes { get; set; }
        public int numNodes { get; set; }
        public int blockSize { get; set; }
        public byte[] Address { get; set; }
        //public List<Node> Nodes { get; set; }
        public Node next { get; set; }
        public Block child { get; set; }
    }
}
