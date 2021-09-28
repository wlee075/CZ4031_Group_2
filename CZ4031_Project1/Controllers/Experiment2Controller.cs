using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    public class Experiment2Controller
    {
        public static int IndexOfRecordToBeInserted { get; set; }
        public static Block CurrentRecordBlock = BPlusTreeController.CurrentRecordBlock;
        public int levels { get; set; }
        public void BuildTree()
        {
            var addresses = MemoryAddressController.GetAddresses().ToArray();
            int blockOffsetSize = addresses.Last().Key.Length;
            int MaxKeys = BPlusTreeController.GetMaxKeys();
            BPlusTree tree = new BPlusTree();
            tree.MaxKeys = MaxKeys;
            IndexOfRecordToBeInserted = 0;

            //if no root exist, create root block
            if (tree.rootBlock == null)
            {
                int counter = 0;
                CurrentRecordBlock = BlockController.CreateBlock();
                CurrentRecordBlock.isLeaf = true; //both root and leaf
                while (counter < tree.MaxKeys)
                {
                    Node node = new Node();
                    node.Key = Convert.ToInt32(addresses[IndexOfRecordToBeInserted].Value.Split('-')[1]);
                    node.Pointer = addresses[IndexOfRecordToBeInserted].Key;
                    CurrentRecordBlock.Nodes.Add(node);
                    MemoryAddressController.InsertValueIntoMemory(BitConverter.ToString(node.Pointer), blockOffsetSize);
                    counter += 1;
                    IndexOfRecordToBeInserted += 1;
                }
                tree.rootBlock = CurrentRecordBlock;
            }
            else
            {
                while (IndexOfRecordToBeInserted < addresses.Count())
                {
                    int recordToBeInserted = Convert.ToInt32(addresses[IndexOfRecordToBeInserted].Value.Split('-')[1]);
                    byte[] addressofRecordToBeInserted = addresses[IndexOfRecordToBeInserted].Key;

                    BPlusTreeController.InsertBlockIntoBPlusTree(tree, recordToBeInserted, addressofRecordToBeInserted);

                    IndexOfRecordToBeInserted += 1;
                }
            }
            levels = BPlusTreeController.GetLevels(tree);
            Console.WriteLine("Root and child contents: ");
            BPlusTreeController.displayTree(tree.rootBlock.Address, levels);

        }

    }
}
