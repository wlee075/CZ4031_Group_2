﻿using CZ4031_Project1.Entities;
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
        public int numNodes { get; set; }
        public void BuildTree()
        {
            BPlusTree tree = new BPlusTree();
            var addresses = MemoryAddressController.GetAddresses().ToArray();
            IndexOfRecordToBeInserted = 0;
            while (IndexOfRecordToBeInserted < addresses.Count())
            {
                int recordToBeInserted = Convert.ToInt32(addresses[IndexOfRecordToBeInserted].Value.Split('-')[1]);
                byte[] addressofRecordToBeInserted = addresses[IndexOfRecordToBeInserted].Key;
                
                BPlusTreeController.insert(tree, recordToBeInserted, addressofRecordToBeInserted);

                IndexOfRecordToBeInserted += 1;
            }

            numNodes = tree.NumNodes;
            levels = BPlusTreeController.PrintTree(tree);
            Console.WriteLine("levels {0}", levels);

           
        }

    }
}
