using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    public abstract class BPlusTreeController
    {
        // Search for keys corresponding to a range in the B+ Tree given a lower and upper bound. Returns a list of matching Records.
        public abstract void search(int lowerBoundKey, int upperBoundKey);

        // Inserts a record into the B+ Tree.
        public abstract void insert(Block address, int key);

        // Prints out the B+ Tree in the console.
        public abstract void display(Block address, int level);

        // Prints out a specific node and its contents in the B+ Tree.
        public abstract void displayNode(ref Node node);

        // Prints out a data block and its contents in the disk.
        public abstract void displayBlock(ref Block block);

        public int getLevels(BPlusTree tree)
        {
            if (tree.rootAddress == null)
            {
                return 0;
            }

            int levels = 1;

            List<Node> nodes = tree.Blocks.Where(i => i.Address == tree.rootAddress).Select(j => j.Nodes).FirstOrDefault();
            try
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (!nodes[i].IS_LEAF)
                    {
                        levels++;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception found: "+ e);
            }
            
            // Account for linked list (count as one level)
            levels++;

            return levels;
        }

    }
}
