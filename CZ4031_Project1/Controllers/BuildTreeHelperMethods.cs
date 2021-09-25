using CZ4031_Project1.Entities;
using CZ4031_Project1.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    class BuildTreeHelperMethods
    {

        /*
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
        */
        public void SplitChild(ref Block currBlock, ref Block rootBlock)
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
            rightBlock.ParentBlock = currBlock.ParentBlock;

            for (i = x, j = 0; i < numberOfPointers; i++, j++)
            {
                //take the right-half values from curBlocks and put in the rightBlock
                rightBlock.Nodes[i].Key = currBlock.Nodes[i].Key;
                //and erase right-half values from curBlock to make it real leftBlock
                //so that it does not contain all values only contains left-half values
                currBlock.Nodes[i].Key = 0;
            }

            //for splitting the leaf blocks we copy the first item from the rightBlock to their parentBlock
            //and val contains that value
            int val = rightBlock.Nodes[0].Key;

            //if the leaf itself is a parent then
            if (currBlock.ParentBlock == null)
            {
                //it has null parent, so create a new parent
                Block parentBlock = new Block();
                //and new parent should have a null parent
                parentBlock.ParentBlock = null;
                //new parent will have only one member
                parentBlock.numNodes = 1;
                //and that member is val
                parentBlock.Nodes[0].Key = val;
                //so the parent has two child, so assign them (don't forget curBlock is actually the leftBlock)
                parentBlock.ChildBlock[0] = currBlock;
                parentBlock.ChildBlock[1] = rightBlock;
                //their parent of the left and right blocks is no longer null, so assign their parent
                currBlock.ParentBlock = rightBlock.ParentBlock = parentBlock;
                //from now on this parentBlock is the rootBlock
                rootBlock  = parentBlock;
                return;
            }

            else
            {
                //if the splitted leaf block is not rootBlock then

                // we have to put the val and assign the rightBlock to the right place in the parentBlock
                // so we go to the parentBlock and from now we consider the curBlock as the parentBlock of the splitted Block

                currBlock = currBlock.ParentBlock;

                //for the sake of insertNodeion sort to put the rightBlock and val in the exact position
                //of th parentBlock [here curBlock] take a new child block and assign rightBlock to it
                Block newChildBlock = new Block();
                newChildBlock = rightBlock;

                //simple insertion sort to put val at the exact position of values[] in the parentBlock [here curBlock]

                for (i = 0; i <= currBlock.numNodes; i++)
                {
                    if (val < currBlock.Nodes[i].Key)
                    {
                        int temp;
                        temp = currBlock.Nodes[i].Key;
                        currBlock.Nodes[i].Key = val;
                        val = temp;
                    }
                }

                //after putting val number of nodes gets increase by one
                currBlock.numNodes++;

                //simple insertNodeion sort to put rightBlock at the exact position
                //of childBlock[] in the parentBlock [here curBlock]

                for (i = 0; i < currBlock.numNodes; i++)
                {
                    if (newChildBlock.Nodes[0].Key < currBlock.ChildBlock[i].Nodes[0].Key)
                    {
                        Block tempBlk = currBlock.ChildBlock[i];
                        currBlock.ChildBlock[i] = newChildBlock;
                        newChildBlock = tempBlk;
                    }
                }
                currBlock.ChildBlock[i] = newChildBlock;

                //we reordered some blocks and pointers, so for the sake of safety
                //all childBlocks' should have their parent updated
                for (i = 0; currBlock.ChildBlock[i] != null; i++)
                {
                    currBlock.ChildBlock[i].ParentBlock = currBlock;
                }
            }

        }

        public static void SplitNonLeaf(ref Block currBlock, ref Block rootBlock)
        {
            int x, i, j;

            //split the less half to the left when numberOfPointer is odd
            //else split equal equal when numberOfPointer is even.  n/2 does it nicely for us
            GetNumNodes(ref currBlock);
            int numberOfPointers = currBlock.numNodes + 1;
            x = numberOfPointers / 2;

            //declare rightBlock and we will use curBlock as the leftBlock
            Block rightBlock = new Block();

            //so leftBlock has x number of nodes
            currBlock.numNodes = x;
            //rightBlock has numberOfPointers-x-1 children, because we won't copy and paste
            //rather delete and paste the first item of the rightBlock
            rightBlock.numNodes = numberOfPointers - x - 1;
            //both children have their common parent
            rightBlock.ParentBlock = currBlock.ParentBlock;

            for (i = x, j = 0; i <= numberOfPointers; i++, j++)
            {
                //copy the right-half members to the rightBlock
                rightBlock.Nodes[j].Key = currBlock.Nodes[i].Key;
                //and also copy their children
                rightBlock.ChildBlock[j] = currBlock.ChildBlock[i];
                //erase the right-half values from curBlock to make it perfect leftBlock
                //which won't contain only left-half values and their children
                currBlock.Nodes[i].Key = 0;
                //erase all the right-half childBlocks from curBlock except the x one
                //because if left child has 3 nodes then it should have 4 childBlocks, so don't delete that child
                if (i != x) currBlock.ChildBlock[i] = null;
            }

            //we will take a copy of the first item of the rightBlock
            //as we will delete that item later from the list
            int val = rightBlock.Nodes[0].Key;

            //just right-shift value[] and childBlock[] by one from rightBlock
            //to have no repeat of the first item for non-leaf Block
            ShiftRight(rightBlock.Nodes,1);
            ShiftRight(rightBlock.ChildBlock, 1);

            //we reordered some values and positions so don't forget
            //to assign the children's exact parent

            for (i = 0; currBlock.ChildBlock[i] != null; i++)
            {
                currBlock.ChildBlock[i].ParentBlock = currBlock;
            }
            for (i = 0; rightBlock.ChildBlock[i] != null; i++)
            {
                rightBlock.ChildBlock[i].ParentBlock = rightBlock;
            }

            //if the splitted block itself a parent
            if (currBlock.ParentBlock == null)
            {
                //create a new parent
                Block parentBlock = new Block();
                //parent should have a null parent
                parentBlock.ParentBlock = null;
                //parent will have only one node
                parentBlock.numNodes = 1;
                //the only value is the val
                parentBlock.Nodes[0].Key = val;
                //it has two children, leftBlock and rightBlock
                parentBlock.ChildBlock[0] = currBlock;
                parentBlock.ChildBlock[1] = rightBlock;

                //and both rightBlock and leftBlock has no longer null parent, they have their new parent
                currBlock.ParentBlock = rightBlock.ParentBlock = parentBlock;

                //from now on this new parent is the root parent
                rootBlock = parentBlock;
                return;
            }
            else
            {   //if the splitted leaf block is not rootBlock then

                // we have to put the val and assign the rightBlock to the right place in the parentBlock
                // so we go to the parentBlock and from now we consider the curBlock as the parentBlock of the splitted Block
                currBlock = currBlock.ParentBlock;

                //for the sake of insertNodeion sort to put the rightBlock and val in the exact position
                //of th parentBlock [here curBlock] take a new child block and assign rightBlock to it

                Block newChildBlock = new Block();
                newChildBlock = rightBlock;

                //simple insertion sort to put val at the exact position of values[] in the parentBlock [here curBlock]


                for (i = 0; i <= currBlock.numNodes; i++)
                {
                    if (val < currBlock.Nodes[i].Key)
                    {
                        int temp;
                        temp = currBlock.Nodes[i].Key;
                        currBlock.Nodes[i].Key = val;
                        val = temp;
                    }
                }

                //after putting val number of nodes gets increase by one
                currBlock.numNodes++;

                //simple insertNodeion sort to put rightBlock at the exact position
                //of childBlock[] in the parentBlock [here curBlock]

                for (i = 0; i < currBlock.numNodes; i++)
                {
                    if (newChildBlock.Nodes[0].Key < currBlock.ChildBlock[i].Nodes[0].Key)
                    {
                        Block tempBlk = currBlock.ChildBlock[i];
                        currBlock.ChildBlock[i] = newChildBlock;
                        newChildBlock = tempBlk;
                    }
                }

                currBlock.ChildBlock[i] = newChildBlock;

                //we reordered some blocks and pointers, so for the sake of safety
                //all childBlocks' should have their parent updated
                for (i = 0; currBlock.ChildBlock[i] != null; i++)
                {
                    currBlock.ChildBlock[i].ParentBlock = currBlock;
                }
            }
        }

        public static void SplitLeaf(ref Block curBlock, ref Block rootBlock)
        {
            int x, i, j;
            GetNumNodes(ref curBlock);
            int numberOfPointers = curBlock.numNodes + 1;
            //split the greater half to the left when numberOfPointer is odd
            //else split equal equal when numberOfPointer is even
            if (numberOfPointers % 2 == 0)
                x = (numberOfPointers + 1) / 2;
            else x = numberOfPointers / 2;

            //we don't declare another block for leftBlock, rather re-use curBlock as leftBlock and
            //take away the right half values to the rightBlock
            Block rightBlock = new Block();

            //so leftBlock has x number of nodes
            curBlock.numNodes = x;
            //and rightBlock has numberOfPointers-x
            rightBlock.numNodes = numberOfPointers - x;
            //so both of them have their common parent [even parent may be null, so both of them will have null parent]
            rightBlock.ParentBlock = curBlock.ParentBlock;

            for (i = x, j = 0; i < numberOfPointers; i++, j++)
            {
                //take the right-half values from curBlocks and put in the rightBlock
                rightBlock.Nodes[j].Key = curBlock.Nodes[i].Key;
                //and erase right-half values from curBlock to make it real leftBlock
                //so that it does not contain all values only contains left-half values
                curBlock.Nodes[i].Key = 0;
            }
            //for splitting the leaf blocks we copy the first item from the rightBlock to their parentBlock
            //and val contains that value
            int val = rightBlock.Nodes[0].Key;

            //if the leaf itself is a parent then
            if (curBlock.ParentBlock == null)
            {
                //it has null parent, so create a new parent
                Block parentBlock = new Block();
                //and new parent should have a null parent
                parentBlock.ParentBlock = null;
                //new parent will have only one member
                parentBlock.numNodes = 1;
                //and that member is val
                parentBlock.Nodes[0].Key = val;
                //so the parent has two child, so assign them (don't forget curBlock is actually the leftBlock)
                parentBlock.ChildBlock[0] = curBlock;
                parentBlock.ChildBlock[1] = rightBlock;
                //their parent of the left and right blocks is no longer null, so assign their parent
                curBlock.ParentBlock = rightBlock.ParentBlock = parentBlock;
                //from now on this parentBlock is the rootBlock
                rootBlock = parentBlock;
                return;
            }
            else
            {
                // we have to put the val and assign the rightBlock to the right place in the parentBlock
                // so we go to the parentBlock and from now we consider the curBlock as the parentBlock of the splitted Block

                curBlock = curBlock.ParentBlock;

                //for the sake of insertNodeion sort to put the rightBlock and val in the exact position
                //of th parentBlock [here curBlock] take a new child block and assign rightBlock to it
                Block newChildBlock = new Block();
                newChildBlock = rightBlock;
                //simple insertion sort to put val at the exact position of values[] in the parentBlock [here curBlock]

                for (i = 0; i <= curBlock.numNodes; i++)
                {
                    if (val < curBlock.Nodes[i].Key)
                    {
                        int temp;
                        temp = curBlock.Nodes[i].Key;
                        curBlock.Nodes[i].Key = val;
                        val = temp;
                    }
                }

                //after putting val number of nodes gets increase by one
                curBlock.numNodes++;

                //simple insertNodeion sort to put rightBlock at the exact position
                //of childBlock[] in the parentBlock [here curBlock]

                for (i = 0; i < curBlock.numNodes; i++)
                {
                    if (newChildBlock.Nodes[0].Key < curBlock.ChildBlock[i].Nodes[0].Key)
                    {
                        Block tempBlk = curBlock.ChildBlock[i];
                        curBlock.ChildBlock[i] = newChildBlock;
                        newChildBlock = tempBlk;
                    }
                }
                curBlock.ChildBlock[i] = newChildBlock;

                //we reordered some blocks and pointers, so for the sake of safety
                //all childBlocks' should have their parent updated
                for (i = 0; curBlock.ChildBlock[i] != null; i++)
                {
                    curBlock.ChildBlock[i].ParentBlock = curBlock;
                }
            }
        }


        


        public static void ShiftRight<Node>(List<Node> lst, int shifts)
        {
            for (int i = lst.Count - shifts - 1; i >= 0; i--)
            {
                lst[i + shifts] = lst[i];
            }

            for (int i = 0; i < shifts; i++)
            {
                lst[i] = default(Node);
            }
        }

        public void GetMaxKeys(ref Block currentBlock)
        {
            int maxKeys, numNodesInBlock;
            maxKeys = numNodesInBlock = 0;
            int blockSize = BlockController.BlockSize;
            int blockAddress = BlockController.BlockAddressSize;
            // Get size left for keys and pointers in a node after accounting for node's isLeaf 
            int nodeBufferSize = blockSize - sizeof(bool);
            // Get size of memory address
            int currentMemAddrSize = blockAddress + sizeof(int) + numNodesInBlock * sizeof(int);
            while(currentMemAddrSize + ((numNodesInBlock+1) * sizeof(int)) <= nodeBufferSize)
            {
                ++numNodesInBlock;
                currentMemAddrSize += numNodesInBlock * sizeof(int);
                maxKeys += 1;
            }
            currentBlock.MaxKeys = maxKeys;
        }

        public static void GetNumNodes(ref Block currentBlock)
        {
            currentBlock.numNodes = currentBlock.Nodes.Count();
        }

        public void SetChildBlocksNumber(ref Block currentBlock)
        {
            GetMaxKeys(ref currentBlock);
            currentBlock.Nodes = new List<Node>(currentBlock.MaxKeys);
        }


    }
}
