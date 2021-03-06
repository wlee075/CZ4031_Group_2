using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    public class BPlusTreeController
    {
        //static Block block { get; set; }
        static Node node { get; set; }
        static int minKeys { get; set; }
        static int nodeCount { get; set; }
        public static List<Block> TraversedBlocks { get; set; }
        public static int  DeletedAmount {get;set;}
        public static void DeleteNode(int key)
        {

            DeletedAmount = 0;
            var tree = Experiment2Controller.tree;
            var blocks = BlockController.traverseGetBlockList(tree.rootBlock, key);
            TraversedBlocks = blocks;

            Block leafBlock = TraversedBlocks.Last();
            node = BlockController.FindNode(leafBlock, key);
            minKeys = Experiment2Controller.tree.getMinKeys() - 1;
            nodeCount = BlockController.GetNodeCount(leafBlock);


            //if has less than minimum keys aftering deleting
            if (BlockController.GetNodeCount(leafBlock) - 1 <= minKeys)
            {
                //Borrow Keys from next sibling
                var lastNodeInBlock = BlockController.FindLastNodeInBlock(leafBlock);
                var nextBlock = lastNodeInBlock.nextBlock;
                var nextBlockNode = nextBlock.Next;
                List<Block> siblingTraveredBlocks = BlockController.traverseGetBlockList(Experiment2Controller.tree.rootBlock, nextBlockNode.Key);
                Node copyNode = new Node();
                copyNode.Key = nextBlockNode.Key;
                copyNode.nextBlock = nextBlock;

                lastNodeInBlock.next = copyNode;
                lastNodeInBlock.nextBlock = null;

                BlockController.DeleteAndShift(siblingTraveredBlocks, nextBlock, copyNode.Key);

            }
            //Shifting of nodes in current block
            BlockController.DeleteAndShift(TraversedBlocks, leafBlock, key);
            
        }
        public static List<Block> GetCurrentTraversedBlocks()
        {
            return TraversedBlocks;
        }

        public static void insert(BPlusTree tree, int key, MemoryAddress address)
        {
            // empty tree
            if (tree.rootBlock == null)
            {
                tree.rootBlock = BlockController.createBlock(tree.blockSize);
                tree.rootBlock.Next = NodeController.createNode(key, address);
                tree.rootBlock.NumNodes++;
                return;
            }
            //if root has no child
            else if(tree.rootBlock.Child == null)
            {

                if (tree.rootBlock.NumNodes < tree.rootBlock.MaxNodes)
                {
                    BlockController.appendKeyToBlock(tree.rootBlock, key, address);
                }
                else
                {
                    Block newBlock = BlockController.splitBlock(tree.rootBlock, key, address);
                    if (newBlock == null) {  return; } // duplicate key, no split occur
                    else
                    {
                        Block newRoot = BlockController.createBlock(tree.blockSize);
                        BlockController.appendKeyToBlock(newRoot, newBlock.Next.Key, newBlock);
                        newRoot.Child = tree.rootBlock;
                        tree.rootBlock = newRoot;
                    }
                }
                return;
            }
            
            Stack<Block> path = BlockController.traverseToLeaf(tree.rootBlock,key);
            Block currBlock = path.Pop();

            if (currBlock.NumNodes == currBlock.MaxNodes)
            {
                Block newBlock = BlockController.splitBlock(currBlock, key, address);
                Block nBlock = null;
                if (newBlock == null)
                {
                    return;
                }
                Block parentBlock = path.Pop();
                while (parentBlock.NumNodes == parentBlock.MaxNodes)
                {

                    nBlock = BlockController.splitBlock(parentBlock, BlockController.findSmallest(newBlock), newBlock);
                    newBlock = nBlock;

                    if (path.Count == 0)
                    {
                        tree.rootBlock = BlockController.createBlock(tree.blockSize);
                        tree.rootBlock.Child = parentBlock;
                        parentBlock = tree.rootBlock;
                        break;
                    }

                    parentBlock = path.Pop();
                }
                if (path.Count == 0)
                {
                    BlockController.appendKeyToBlock(parentBlock, BlockController.findSmallest(newBlock), newBlock);
                }
                else
                {
                    BlockController.appendKeyToBlock(parentBlock, BlockController.findSmallest(newBlock), newBlock);
                }

                
            }
            else if (currBlock.NumNodes < currBlock.MaxNodes)
            {
                BlockController.appendKeyToBlock(currBlock, key, address);
            }

        }
        public static BPlusTree createTree(int blockSize)
        {
            BPlusTree tree = new BPlusTree();
            tree.blockSize = blockSize;
            tree.rootBlock = null;
            return tree;
        }
        public static void printTree(BPlusTree tree)
        {
            Block currBlock = tree.rootBlock;
            Block presBlock = tree.rootBlock;
            while (currBlock != null)
            {
                Node currNode = currBlock.Next;
                Console.Write("|");
                while(currNode != null)
                {
                    Console.Write(currNode.Key);

                    Console.Write(", ");
                    if (currNode.nextBlock != null)
                    {
                        presBlock = currNode.nextBlock;
                        Console.Write("| -> ");
                        Console.Write("(" + presBlock.NumNodes + ")|");
                        currNode = currNode.nextBlock.Next;
                        
                    }
                    else
                    {
                        currNode = currNode.next;
                    }
                }
                
                currBlock = currBlock.Child;
                Console.WriteLine();
            }
        }
        public static int countNodes(BPlusTree tree)
        {
            Block currBlock = tree.rootBlock;
            Block presBlock = tree.rootBlock;
            int nodeCount = 0;
            while (currBlock != null)
            {
                nodeCount++;
                Node currNode = currBlock.Next;
                while (currNode != null)
                {
                    if (currNode.nextBlock != null)
                    {
                        presBlock = currNode.nextBlock;
                        currNode = currNode.nextBlock.Next;
                        nodeCount++;
                    }
                    else
                    {
                        currNode = currNode.next;
                    }
                }

                currBlock = currBlock.Child;
            }
            return nodeCount;
        }
        public static int countInternalNodes(BPlusTree tree)
        {
            Block currBlock = tree.rootBlock;
            Block presBlock = tree.rootBlock;
            int nodeCount = 0;
            while (currBlock != null)
            {
                Node currNode = currBlock.Next;
                while (currNode != null)
                {
                    nodeCount++;
                    if (currNode.nextBlock != null)
                    {
                        presBlock = currNode.nextBlock;
                        currNode = currNode.nextBlock.Next;

                    }
                    else
                    {
                        currNode = currNode.next;
                    }
                }

                currBlock = currBlock.Child;
            }
            return nodeCount;
        }
        
        //for Experiment 3
        public static List<MemoryAddress> retrieveMovie(BPlusTree tree, int numVote)
        {
            int i = 0;

            Stack<Block> path = BlockController.traverseToLeaf(tree.rootBlock, numVote);

            //get num and content of data blocks
            List<Block> blockList = BlockController.traverseGetBlockList(tree.rootBlock, numVote);

            Console.WriteLine("Displaying first 5 records...");

            foreach (Block b in blockList)
            {
                if (i == 5)
                {
                    break;
                }
                Console.Write("Content of block[" + i + "]: ");
                BlockController.printBlock(b);
                i++;
            }

            Console.WriteLine("Num of blocks accessed: " + path.Count);

            Console.WriteLine("");

            //get num and content of index nodes
            List<Node> nodeList = BlockController.traverseGetNodeList(tree.rootBlock, numVote);

            int j = 0;

            foreach (Node n in nodeList)
            {
                if (j == 5)
                {
                    break;
                }
                Console.Write("Content of node[" + j + "]: ");
                NodeController.printNode(n);
                j++;
            }

            Console.WriteLine("Num of nodes accessed: " + nodeList.Count);

            Console.WriteLine("");

            Node node = path.Pop().Next;

            while (node != null)
            {

                if (node.Key == numVote)
                {
                    return node.Address;
                }

                node = node.next;
            }
            return null;
        }

        //for Experiment 4
        public static List<MemoryAddress> retrieveMovieRange(BPlusTree tree, int min_numVote, int max_numVote)
        {
            int i = 0;
            int n = 0;
            //Stack<Block> path = BlockController.traverseToLeaf_range(tree.rootBlock, min_numVote);

            //get num and content of data blocks
            List<Block> blockList = BlockController.traverseGetBlockList_range(tree.rootBlock, min_numVote, max_numVote);
            List<MemoryAddress> records = new List<MemoryAddress>();

            Console.WriteLine("Displaying first 5 records...");

            foreach (Block b in blockList)
            {
                if (i == 5)
                {
                    break;
                }
                Console.Write("Content of block[" + i + "]: ");
                BlockController.printBlock(b);
                i++;

            }

            Console.WriteLine("Num of blocks accessed: " + blockList.Count);

            Console.WriteLine("");

            foreach (Block b in blockList)
            {
                Node currNode = b.Next;
                while (currNode != null)
                {
                    if (n < 5)
                    {
                        Console.WriteLine("Content of node[" + n + "]: " + currNode.Key);
                    }

                    n++;

                    records.AddRange(currNode.Address);
                    if (currNode.Key > min_numVote || (currNode.Key > max_numVote && b.Child == null))
                    {
                        break;
                    }

                    currNode = currNode.next;
                }
            }

            Console.WriteLine("Num of nodes accessed: " + n);

            Console.WriteLine("");

            return records;
        }
    }


}
