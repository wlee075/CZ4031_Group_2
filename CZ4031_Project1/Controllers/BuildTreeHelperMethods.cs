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
        public void insertRecordIntoTree(BPlusTree tree, Block block, int key)
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
                tree.Blocks.Add(currBlock);
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
                if (currBlock.Nodes.Count < tree.MaxKeys)
                {
                    int i = 0;

                    // Iterate through the parent to see where to put in the lower bound key for the new child.
                    while (key > currBlock.Nodes[i].Key && i < currBlock.Nodes.Count)
                    {
                        i++;
                    }

                    // Now we have i, the index to insert the key in. Bubble swap all keys back to insert the new child's key.
                    // We use numKeys as index since we are going to be inserting a new key.
                    for (int j = currBlock.Nodes.Count; j > i; j--)
                    {
                        currBlock.Nodes[j].Key = currBlock.Nodes[j - 1].Key;
                    }

                    // Shift all pointers one step right (right pointer of key points to lower bound of key).
                    for (int j = currBlock.Nodes.Count + 1; j > i + 1; j--)
                    {
                        currBlock.Nodes[j].Pointer = currBlock.Nodes[j - 1].Pointer;
                    }

                    // Add in new child's lower bound key and pointer to the parent.
                    currBlock.Nodes[i].Key = key;

                    // Right side pointer of key of parent will point to the new child node.
                    currBlock.Nodes[i + 1].Pointer = currBlock.Address;
                }
                // If parent node doesn't have space, we need to recursively split parent node and insert more parent nodes.
                else
                {

                }
            }
        }
    }
}
