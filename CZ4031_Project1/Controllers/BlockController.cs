using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    public static class BlockController
    {
        static int BlockCounter = 1;
        public static Block createBlock(int blockSize)
        {
            Block newBlock = new Block();
            newBlock.Id = BlockCounter;
            int numberOfNode = (blockSize - 16)/Node.nodeSize;
            newBlock.maxNodes = numberOfNode;
            newBlock.numNodes = 0;
            newBlock.blockSize = blockSize;
            //newBlock.Nodes = new List<Node>();
            newBlock.next = null;
            newBlock.child = null;
            BlockCounter += 1;
            return newBlock;
        }
        public static Block splitBlock(Block currBlock, int key, MemoryAddress address)
        {
            Node currNode = currBlock.next;
            int n = currBlock.numNodes;
            int split_n = (n + 1) / 2 + (n+1) % 2;
            Node splitNode = null;
            BlockController.appendKeyToBlock(currBlock,key,address);
            if (n == currBlock.numNodes)
            {
                return null;
            }
            while (currNode != null)
            {
                if (split_n == 1)
                {
                    splitNode = currNode;
                }
                currNode = currNode.next;
                split_n--;
            }
            Block newBlock = BlockController.createBlock(currBlock.blockSize);
            
            currNode = splitNode.next;
            splitNode.next = null;
            currBlock.numNodes = (n + 1) / 2 + (n + 1) % 2; 

            newBlock.next = currNode;
            newBlock.numNodes = (n + 1) / 2;

            while (currNode.next != null)
            {
                currNode = currNode.next;
            }

            
            splitNode.nextBlock = newBlock;

            return newBlock;
        }
        public static Block splitBlock(Block currBlock, int key, Block child)
        {
            
            int n = currBlock.numNodes;
            int split_n = n/2 + n%2;
            Node splitNode = null;
            BlockController.appendKeyToBlock(currBlock,key, child);
            Node currNode = currBlock.next;
            while (currNode != null)
            {

                if (split_n == 1)
                {
                    splitNode = currNode;
                }
                currNode = currNode.next;
                split_n--;
            }
            Block newBlock = BlockController.createBlock(currBlock.blockSize);

            currNode = splitNode.next;
            splitNode.next = null;
            currBlock.numNodes = n / 2 + n % 2;
            newBlock.child = currNode.child;

            currNode = currNode.next;
            newBlock.next = currNode;
            newBlock.numNodes = n / 2;
            while (currNode.next != null)
            {
                currNode = currNode.next;
            }
            //currNode.nextBlock = splitNode.nextBlock;
            splitNode.nextBlock = newBlock;
            return newBlock;
        }
        public static void appendKeyToBlock(Block currBlock, int key, MemoryAddress address)
        {
            if (currBlock.next == null)
            {
                Node node = NodeController.createNode(key, address);
                currBlock.next = node;
                currBlock.numNodes = 1;
            }
            else
            {
                Node prevNode = null;
                Node currNode = currBlock.next;

                while(currNode.Key < key && currNode.next != null)
                {
                    prevNode = currNode;
                    currNode = currNode.next;
                }
                if (currNode.Key == key)
                {
                    currNode.Address.Add(address);
                }
                else if (currNode.Key < key)
                {
                    currNode.next = NodeController.createNode(key, address);
                    currNode.next.nextBlock = currNode.nextBlock;
                    currNode.nextBlock = null;
                    currBlock.numNodes++;
                }
                else
                {
                    if (prevNode != null)
                    {
                        prevNode.next = NodeController.createNode(key,address);
                        prevNode = prevNode.next;
                        prevNode.next = currNode;
                        currBlock.numNodes++;
                    }
                    else
                    {
                        prevNode = NodeController.createNode(key,address);
                        prevNode.next = currNode;
                        currBlock.numNodes++;
                        currBlock.next = prevNode;
                    }
                }
            }

        }
        public static void appendKeyToBlock(Block currBlock, int key, Block child)
        {
            if (currBlock.next == null)
            {
                Node node = NodeController.createNode(key);
                node.child = child;
                currBlock.next = node;
                currBlock.numNodes = 1;
            }
            else
            {
                Node prevNode = null;
                Node currNode = currBlock.next;
                while (currNode.Key < key && currNode.next != null)
                {
                    prevNode = currNode;
                    currNode = currNode.next;
                }
                if (currNode.Key == key)
                {
                    Console.WriteLine("Error, logic not suppose to enter here");
                }
                if (currNode.Key < key)
                {
                    currNode.next = NodeController.createNode(key);
                    currNode.next.nextBlock = currNode.nextBlock;
                    currNode.next.child = child;
                    currNode.nextBlock = null;
                    currBlock.numNodes++;
                }
                else
                {
                    if (prevNode != null)
                    {
                        prevNode.next = NodeController.createNode(key);
                        prevNode.next.child = child;
                        prevNode.next.nextBlock = prevNode.nextBlock;
                        prevNode.nextBlock = null;
                        prevNode = prevNode.next;
                        prevNode.next = currNode;
                        currBlock.numNodes++;
                    }
                    else
                    {
                        prevNode = NodeController.createNode(key);
                        prevNode.child = child;
                        prevNode.next = currNode;
                        currBlock.next = prevNode;
                        currBlock.numNodes++;
                    }
                }
            }
        }
        public static int findSmallest(Block currBlock)
        {
            while (currBlock.child != null)
            {
                currBlock = currBlock.child;
            }
            return currBlock.next.Key;
        }
        public static Stack<Block> traverseToLeaf(Block currBlock, int key)
        {
            Stack<Block> path = new Stack<Block>();
            path.Push(currBlock);
            Node currNode = null;
            Node prevNode = null;

            while (currBlock.child != null)
            {
                prevNode = null;
                currNode = currBlock.next;
                while (currNode != null)
                {
                    if (currNode.Key > key)
                    {
                        break;
                    }
                    prevNode = currNode;
                    currNode = currNode.next;
                }
                if (prevNode == null)
                {
                    currBlock = currBlock.child;
                }
                else
                {
                    currBlock = prevNode.child;

                }
                //Console.WriteLine("pushing " + currBlock.next.Key);
                
                path.Push(currBlock);
                
            }
            return path;
        }

        public static Stack<Block> traverseToLeaf_range(Block currBlock, int key)
        {
            Stack<Block> path = new Stack<Block>();
            path.Push(currBlock);
            Node currNode = null;
            Node prevNode = null;

            while (currBlock.child != null)
            {
                prevNode = null;
                currNode = currBlock.next;
                while (currNode != null)
                {
                    if (currNode.Key > key)
                    {
                        break;
                    }
                    prevNode = currNode;
                    currNode = currNode.next;
                }
                if (prevNode == null)
                {
                    currBlock = currBlock.child;
                }
                else
                {
                    currBlock = prevNode.child;

                }
                //Console.WriteLine("pushing " + currBlock.next.Key);

                path.Push(currBlock);

            }



            return path;
        }

        public static List<Block> traverseGetBlockList_range(Block currBlock, int min_key, int max_key)
        {
            List<Block> blockList = new List<Block>();
            blockList.Add(currBlock);

            Stack<Block> path = new Stack<Block>();
            path.Push(currBlock);
            Node currNode = null;
            Node prevNode = null;

            while (currBlock.child != null)
            {
                prevNode = null;

                currNode = currBlock.next;
                while (currNode != null)
                {
                    if (currNode.Key > min_key)
                    {
                        break;
                    }
                    prevNode = currNode;
                    currNode = currNode.next;

                }

                if (prevNode == null)
                {
                    currBlock = currBlock.child;
                }
                else
                {
                    currBlock = prevNode.child;

                }
                //Console.WriteLine("pushing " + currBlock.next.Key);

                blockList.Add(currBlock);

                path.Push(currBlock);
            }
            currNode = currBlock.next;

            //leaf node, traverse to right
            while(currNode != null)
            {
                while(currNode.next != null)
                {
                    currNode = currNode.next;
                }
                if (currNode.Key >= max_key)
                {
                    break;
                }
                currBlock = currNode.nextBlock;
                blockList.Add(currBlock);
                currNode = currBlock.next;
            }


            return blockList;
        }

        public static List<Block> traverseGetBlockList(Block currBlock, int key)
        {
            List<Block> blockList = new List<Block>();
            blockList.Add(currBlock);

            Stack<Block> path = new Stack<Block>();
            path.Push(currBlock);
            Node currNode = null;
            Node prevNode = null;

            while (currBlock.child != null)
            {
                prevNode = null;
                currNode = currBlock.next;
                while (currNode != null)
                {
                    if (currNode.Key > key)
                    {
                        break;
                    }
                    prevNode = currNode;
                    currNode = currNode.next;
                }
                if (prevNode == null)
                {
                    currBlock = currBlock.child;
                }
                else
                {
                    currBlock = prevNode.child;

                }
                //Console.WriteLine("pushing " + currBlock.next.Key);

                blockList.Add(currBlock);

                path.Push(currBlock);
            }
            

            return blockList;
        }

        public static List<Node> traverseGetNodeList(Block currBlock, int key)
        {
            List<Node> nodeList = new List<Node>();

            Stack<Block> path = new Stack<Block>();
            path.Push(currBlock);
            Node currNode = null;
            Node prevNode = null;

            if(currNode != null)
            {
                //add root node
                nodeList.Add(currNode);
            }

            while (currBlock.child != null)
            {
                prevNode = null;
                currNode = currBlock.next;

                while (currNode != null)
                {
                    //add node if not null
                    nodeList.Add(currNode);
                    if (currNode.Key > key)
                    {
                        break;
                    }
                    prevNode = currNode;
                    currNode = currNode.next;
                }
                if (prevNode == null)
                {
                    currBlock = currBlock.child;
                }
                else
                {
                    currBlock = prevNode.child;
                }
                //Console.WriteLine("pushing " + currBlock.next.Key);

                path.Push(currBlock);

            }

            Node last_node = currBlock.next;

            while (last_node.next != null)
            {
                if (last_node.Key >= key)
                {
                    break;
                }

                nodeList.Add(last_node);
                last_node = last_node.next;
            }

            //last node of block
            nodeList.Add(last_node);

            return nodeList;
        }

        public static void printBlock(Block block)
        {
            Node node = block.next;
            while(node != null)
            {
                Console.Write(node.Key);
                node = node.next;
                if (node != null)
                {
                    Console.Write(" -> ");
                }
            }
            Console.WriteLine();
        }
        public static Block FindBlock(List<Block> blocks, int key)
        {
            foreach(var b in blocks)
            {
                var node = FindNode(b, key);
                if(node != null)
                {
                    return b;
                }
            }
            return null;
        }
        public static Node FindNode(Block block, int key)
        {
            Node currNode = block.next;
            while (currNode != null)
            {
                if (currNode.Key == key)
                {
                    return currNode;
                }
                currNode = currNode.next;
            }
            return null;
        }
        public static Node FindLastNodeInBlock(Block block)
        {
            Node currNode = block.next;
            while (currNode.next != null)
            {
                currNode = currNode.next;
            }
            return currNode;
        }
        public static Node FindPrevNode(Block block, int key)
        {
            Node prevNode = null;
            Node currNode = block.next;
            while (currNode != null)
            {
                if (currNode.Key == key)
                {
                    return prevNode;
                }
                prevNode = currNode;
                currNode = currNode.next;
            }
            return null;
        }
        public static void DeleteAndShift(Block block, int key)
        {
            var prevNode = FindPrevNode(block, key);
            var currNode = FindNode(block, key);
            //if previous node exists
            if (prevNode != null)
            {
                //deletes and points to next
                prevNode.next = currNode.next;
                currNode = null;
            }
            else
            {
                var nextNode = currNode.next;
                currNode.Key = nextNode.Key;
                currNode.nextBlock = nextNode.nextBlock;
                currNode.next = nextNode.next;
                nextNode = null;

                //Update parent block
            }
        }
        public static int GetNodeCount(Block block)
        {
            int total = 1;
            Node currNode = block.next;
            while (currNode.next != null)
            {
                total += 1;
                currNode = currNode.next;
            }
            return total;
        }
        public static Block GetParentBlock(List<Block> blocks, Block block)
        {
            Block parentBlock = null;
            for(int i = 0; i<blocks.Count; i++)
            {
                int nextI = i + 1;
                if (nextI < blocks.Count)
                {

                    if (blocks[nextI].Id == block.Id)
                    {

                        parentBlock = blocks[i];
                    }
                }
                
            }
            return parentBlock;
        }
    }
}
