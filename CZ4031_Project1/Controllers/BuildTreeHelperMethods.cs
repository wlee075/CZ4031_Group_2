using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    class BuildTreeHelperMethods
    {
        // key: record's numVotes, value: {Block addr, offset}
        public void insertRecordIntoTree(Block block, int key)
        {
            Block currBlock = block;
            Node currNode = new Node
            {
                Pointer = block.Address
            };
            //if no root exist, create B+ tree root
            if (block.Address == null)
            {
                // create node in MM 
                currNode.Key = key;
                currNode.IS_LEAF = true; //is both root and leaf
                currBlock.Nodes.Add(currNode);
            }
            else
            {
                // traverse nodes to find proper place to insert key
                Node parentNode = new Node();
                parentNode.Pointer = block.Address;
                while (!currNode.IS_LEAF)
                {
                    // set parent node and its address
                    parentNode = currNode;
                    parentNode.Pointer = currNode.Pointer;

                    for (int i = 0; i < currBlock.Nodes.Count; i++)
                    {

                        currBlock.Pointer = currBlock.Address;

                        //if key < current key, go to left pointer's block
                        if (key < currBlock.Nodes[i].Key)
                        {
                            currBlock.Address = currBlock.Nodes[i].Pointer;     
                            break;
                        }

                        // if key larger than all keys in block, go to last pointer's block
                        if (i == currBlock.Nodes.Count - 1)
                        {
                            currBlock.Address = currBlock.Nodes[i+1].Pointer;
                            break;
                        }
                    }
                }

                // reached leaf node, if space is available, find location to place it

            }
        }
    }
}
