using CZ4031_Project1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Entities
{
    public class BPlusTree
    {
        public int blockSize { get; set; }
        public Block rootBlock { get; set; }
        public int numNodes { get; set; }
        
        public int getN()
        {
            return rootBlock.maxNodes;
        }
        public int getNumberOfNodes()
        {
            return BPlusTreeController.countNodes(this);
        }
        public int getMinKeys()
        {
            return (int)Math.Ceiling((decimal)rootBlock.maxNodes / 2);
        }
        public int getNumberOfInternalNodes()
        {
            return BPlusTreeController.countInternalNodes(this);
        }
        public int getHeight()
        {
            int h = 0;
            Block currBlock = rootBlock;
            while (currBlock != null)
            {
                currBlock = currBlock.child;
                h++;
            }
            return h;
        }
    }
}
