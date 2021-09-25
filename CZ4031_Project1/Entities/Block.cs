using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Entities
{
    public class Block
    {
        public bool IsRecordBlock { get; set; }
        public int MaxKeys { get; set; }
        public int numNodes { get; set; }
        public string Id { get; set; }
        public byte[] Address { get; set; }
        public Block ParentBlock { get; set; }
        public List<Block> ChildBlock { get; set; }
        public byte[] Pointer { get; set; }
        public List<Node> Nodes { get; set; }

    }
}
