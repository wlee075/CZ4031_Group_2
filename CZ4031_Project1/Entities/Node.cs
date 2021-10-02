using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Entities
{
    public class Node
    {
        //Assume size of key is 8bytes, size of address of the record is 8 bytes
        //, and size of pointer to next and child is 8 bytes each
        public static int nodeSize = 24;
       
        public List<MemoryAddress> Address { get; set; }
        public int Key { get; set; }
        public Node next { get; set; }
        public Block nextBlock { get; set; }
        public Block child { get; set; }
    }
}
