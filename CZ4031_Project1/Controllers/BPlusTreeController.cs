using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    public class BPlusTreeController:BPlusTree
    {
        public static Block CurrentRecordBlock { get; set; }
        public const int NodeAddressSize = BlockController.BlockAddressSize;
        public static Dictionary<byte[], Block> Blocks = BlockController.Blocks;
        public static int MaxKeys = GetMaxKeys();

        public static void InsertBlockIntoBPlusTree(BPlusTree tree, int recordToBeInserted, byte[] addressofRecordToBeInserted)
        {
            Block cursor = tree.rootBlock;

            while (!cursor.isLeaf)
            {
                // Check through all keys of the node to find key and pointer to follow downwards.
                for (int i = 0; i < cursor.Nodes.Count; i++)
                {

                    // If key is lesser than current key, go to the left pointer's node.
                    if (recordToBeInserted < cursor.Nodes[i].Key)
                    {
                        // Load node in from disk to main memory.
                        Block leftChildNode = Blocks[cursor.Nodes[i].Pointer];

                        // Update cursorDiskAddress to maintain address in disk if we need to update nodes.
                        cursor.Address = cursor.Nodes[i].Pointer;

                        // Move to new node in main memory.
                        cursor = leftChildNode;
                        break;
                    }

                    // Else if key larger than all keys in the node, go to last pointer's node (rightmost).
                    if (i == cursor.Nodes.Count - 1)
                    {
                        // Load node in from disk to main memory.
                        Block rightMostChildNode = Blocks[cursor.Nodes[i + 1].Pointer];

                        // Update diskAddress to maintain address in disk if we need to update nodes.
                        cursor.Address = cursor.Nodes[i + 1].Pointer;

                        // Move to new node in main memory.
                        cursor = rightMostChildNode;
                        break;
                    }
                }
            }

            // If this leaf node still has space to insert a key, then find out where to put it.
            if (cursor.Nodes.Count < MaxKeys)
            {
                // Update leaf node pointer link to next node
                InsertLeafNodesIntoTreeBlock(tree, cursor, recordToBeInserted, addressofRecordToBeInserted);
            }
            else
            {
                SplitBlock(tree, ref cursor, recordToBeInserted, addressofRecordToBeInserted);

            }
        }


        public static void InsertInternalNodesIntoTreeBlock(BPlusTree tree, int key, byte[] parentAddress, byte[] ChildAddress)
        {

            Block cursor = Blocks[parentAddress];

            if (cursor.Address == tree.rootBlock.Address)
            {
                tree.rootBlock = cursor;
            }

            Node newNode = new Node();
            newNode.Key = key;
            newNode.Pointer = ChildAddress;

            if (cursor.Nodes.Count < MaxKeys)
            {
                int i = 0;

                while (key > cursor.Nodes[i].Key && i < cursor.Nodes.Count)
                {
                    i++;
                }

                if (key < cursor.Nodes[i].Key)
                {
                    if (i == 0)
                        cursor.Nodes.Insert(0, newNode);
                    else
                        cursor.Nodes.Insert(i - 1, newNode);
                }
                // i is where our key goes in. Check if it's already there (duplicate).
                else if (key == cursor.Nodes[i].Key)
                {
                    cursor.Nodes.Insert(i + 1, newNode);
                }
                else
                {
                    cursor.Nodes.Add(newNode);
                }

            }
            else
            {
                SplitBlock(tree, ref cursor, key, ChildAddress);
            }
        }

        public static void InsertLeafNodesIntoTreeBlock(BPlusTree tree, Block block, int key, byte[] addressofRecordToBeInserted)
        {
            // Console.WriteLine("inserting leaf key {0} into tree with max keys {1}", key, tree.MaxKeys);

            //new block deets
            Node newNode = new Node();
            Block cursor = block;
            newNode.Key = key;
            newNode.Pointer = addressofRecordToBeInserted;

            //leaf traversal
            if (cursor.Nodes.Count < MaxKeys)
            {
                int i = 0;

                // While we haven't reached the last key and the key we want to insert is larger than current key, keep moving forward.
                while (key > cursor.Nodes[i].Key && i < cursor.Nodes.Count)
                {
                    i++;
                }

                if (key < cursor.Nodes[i].Key)
                {
                    if (i == 0)
                        cursor.Nodes.Insert(0, newNode);
                    else
                        cursor.Nodes.Insert(i - 1, newNode);
                }
                // i is where our key goes in. Check if it's already there (duplicate).
                else if (key == cursor.Nodes[i].Key)
                {
                    cursor.Nodes.Insert(i + 1, newNode);
                }
                else
                {
                    cursor.Nodes.Add(newNode);
                }

            }
            else
            {
                SplitBlock(tree, ref cursor, key, addressofRecordToBeInserted);
            }
        }

        public static void SplitBlock(BPlusTree tree, ref Block block, int recordToBeInserted, byte[] addressofRecordToBeInserted)
        {
            Block cursor = block;

            // Split Block
            float cursorNodesCount = (MaxKeys) / 2;
            int SplittedNodeIndex = (int)Math.Floor(cursorNodesCount);
            List<List<Node>> SplittedNodeList = SplitList<Node>(block.Nodes, SplittedNodeIndex).ToList();

            //create Left block
            CurrentRecordBlock = BlockController.CreateBlock();
            CurrentRecordBlock.isLeaf = true;
            Block LeftBlock = CurrentRecordBlock;
            LeftBlock.Nodes = SplittedNodeList[0];

            //create Right block
            CurrentRecordBlock = BlockController.CreateBlock();
            CurrentRecordBlock.isLeaf = true;
            Block RightBlock = CurrentRecordBlock;
            LeftBlock.Nodes = SplittedNodeList[1];

            // create internal block
            CurrentRecordBlock = BlockController.CreateBlock();
            CurrentRecordBlock.isLeaf = false;
            Block InternalBlock = CurrentRecordBlock;
            InternalBlock.Parent = null;
            InternalBlock.Nodes[0].Pointer = RightBlock.Nodes[0].Pointer;

            if (cursor == tree.rootBlock)
            {
                LeftBlock.Pointer = RightBlock.Address;
                LeftBlock.Parent = InternalBlock.Address;
                RightBlock.Parent = InternalBlock.Address;
                tree.rootBlock = InternalBlock;
            }
            else
            {
                LeftBlock.Pointer = RightBlock.Address;
                LeftBlock.Parent = InternalBlock.Address;
                RightBlock.Parent = InternalBlock.Address;
                block = InternalBlock;
            }

            InsertInternalNodesIntoTreeBlock(tree, recordToBeInserted, InternalBlock.Address, addressofRecordToBeInserted);
        }

        public static IEnumerable<List<T>> SplitList<T>(List<T> locations, int nSize)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }

        public static void displayTree(byte[] address, int level)
        {
            Block cursor = Blocks[address];
            
            if (cursor != null)
            {
                for (int i = 0; i < level; i++)
                {
                    Console.Write("  ");
                }
                Console.Write(" level: {0}", level);
                DisplayNode(cursor);
                if (cursor.isLeaf != true)
                {
                    for (int i = 0; i < cursor.Nodes.Count + 1; i++)
                    {
                        // Load node in from disk to main memory.
                        Block mainMemoryNode = Blocks[cursor.Nodes[i].Pointer];
                        displayTree(mainMemoryNode.Address, level + 1);
                    }
                }
            }
        }

        public static void DisplayNode(Block block)
        {

            // Print out all contents in the node as such |pointer|key|pointer|
            int i = 0;
            Console.Write("|");
            for (i = 0; i < block.Nodes.Count; i++)
            {
                Console.Write(" {0} |", block.Nodes[i].Pointer);
                Console.Write(" {0} |", block.Nodes[i].Key);
            }
            // Print last filled pointer
            if (block.Nodes[block.Nodes.Count - 1] == null)
            {
                Console.Write("Null | ");
            }
            else
            {
                Console.Write("{0} |", block.Nodes[block.Nodes.Count - 1].Pointer);
            }
            for (i = block.Nodes.Count; i < MaxKeys; i++)
            {
                Console.Write(" x |");// Remaining empty keys
                Console.Write("  Null  |");// Remaining empty pointers
            }
            Console.WriteLine();
        }



        public static int GetLevels(BPlusTree tree)
        {
            if (tree.rootBlock == null)
            {
                return 0;
            }

            int levels = 1;

            Block root = Blocks[tree.rootBlock.Address];

            Block cursor = root;

            try
            {
                while (!cursor.isLeaf)
                {
                    cursor = Blocks[cursor.Nodes[0].Pointer];
                    levels++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception found: " + e);
            }

            // Account for linked list (count as one level)
            levels++;
            Levels = levels;
            return levels;
        }

        public static int GetMaxKeys()
        {
            decimal maxKeys;
            int blockSize = BlockController.BlockSize;
            int blockAddressSize = BlockController.BlockAddressSize;
            // Get size left for keys and pointers in a node after accounting for node's isLeaf 
            Dictionary<byte[], string> memory = MemoryAddressController.GetAddresses();
            byte[] pointerSize = memory.Keys.Last();
            int keySize = sizeof(int);
            maxKeys = Math.Floor(Convert.ToDecimal(blockSize - blockAddressSize - keySize) / (pointerSize.Length + keySize));
            return (int)maxKeys;
        }

    }
}
