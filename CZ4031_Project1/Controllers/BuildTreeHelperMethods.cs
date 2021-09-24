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
        public void insertRecordIntoTree(Block block, int key, BPlusTree tree)
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
                GetNumNodes(ref currBlock);
                while (!currNode.IS_LEAF)
                {
                    // set parent node and its address
                    parentNode = currNode;
                    parentNode.Pointer = currNode.Pointer;
                    for (int i = 0; i < currBlock.numNodes; i++)
                    {
                        //if key < current key, go to left pointer's node
                        if (key < currBlock.Nodes[i].Key)
                        {
                            currBlock.Address = currBlock.Nodes[i].Pointer;
                            break;
                        }

                        // if key larger than all keys in block, go to last pointer's block
                        if (i == currBlock.numNodes - 1)
                        {
                            currBlock.Address = currBlock.Nodes[i+1].Pointer;
                            break;
                        }
                    }
                }

                // reached leaf node, if space is available, find location to place it
                if (currBlock.numNodes < tree.MaxKeys)
                {
                    int i = 0;

                    // Iterate through the parent to see where to put in the lower bound key for the new child.
                    while (key > currBlock.Nodes[i].Key && i < currBlock.numNodes)
                    {
                        i++;
                    }

                    // Now we have i, the index to insert the key in. Bubble swap all keys back to insert the new child's key.
                    // We use numKeys as index since we are going to be inserting a new key.
                    for (int j = currBlock.numNodes; j > i; j--)
                    {
                        currBlock.Nodes[j].Key = currBlock.Nodes[j - 1].Key;
                    }

                    // Shift all pointers one step right (right pointer of key points to lower bound of key).
                    for (int j = currBlock.numNodes + 1; j > i + 1; j--)
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

        public void SplitChild(ref Block currBlock)
        {
            int x, i, j, numberOfPointers;
            GetNumNodes(ref currBlock);
            numberOfPointers = currBlock.numNodes + 1;
            //split the greater half to the left when numberOfPointer is odd
            //else split equal equal when numberOfPointer is even
            if (numberOfPointers % 2 == 0)
                x = (numberOfPointers + 1) / 2;
            else x = numberOfPointers / 2;

            //we don't declare another block for leftBlock, rather re-use curBlock as leftBlock and
            //take away the right half values to the rightBlock
            Block rightBlock = new Block();

            //so leftBlock has x number of nodes
            currBlock.numNodes = x;
            //and rightBlock has numberOfPointers-x
            rightBlock.numNodes = numberOfPointers - x;
            //so both of them have their common parent [even parent may be null, so both of them will have null parent]
            rightBlock.Address = currBlock.Address;

        }

        public void GetNumNodes(ref Block currentBlock)
        {
            currentBlock.numNodes = currentBlock.Nodes.Count();
        }

        public void SetChildBlocksNumber(ref Block currentBlock)
        {
            currentBlock.MaxKeys = 3;
            currentBlock.Nodes = new List<Node>(currentBlock.MaxKeys);
        }


    }
}
