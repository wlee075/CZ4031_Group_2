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
        public const int BlockSize = 100;
        public const int BlockAddressSize = 10;
        private static double BlockOffsetSize { get; set; }
       // private static Dictionary<string, > RecordBlocks = new List<Block>();
        public static Block CurrentRecordBlock { get; set; }
 

        public static void InsertBlockIntoMemory()
        {
            var addresses = MemoryAddressController.GetAddresses().ToArray();
            int counter = 0;
            int recordsPerBlock = (int)GetRecordsPerBlock();

            while (counter < addresses.Count())
            {
                if(counter % recordsPerBlock == 0)
                {
                    CurrentRecordBlock = CreateBlock();
                }
 
                Node node = new Node();
                node.Key = Convert.ToInt32(addresses[counter].Value.Split('-')[1]);
                node.Pointer = addresses[counter].Key;
                CurrentRecordBlock.Nodes.Add(node);
                MemoryAddressController.InsertValueIntoMemory(BitConverter.ToString(node.Pointer), node.Pointer.Length);
               
                counter += 1;
            }
        }
        public static Block CreateBlock()
        {
            Block newBlock = new Block();
            //If there are no blocks

            if (CurrentRecordBlock == null)
            {
                newBlock = new Block();
                newBlock.Id = "block1";
            }
            else
            {
                //Increase id by 1
                newBlock.Id = String.Format("block{0}", Convert.ToInt32(CurrentRecordBlock.Id.Replace("block", "")) + 1);
            }
            newBlock.Address = MemoryAddressController.InsertValueIntoMemory(newBlock.Id, BlockAddressSize);
            newBlock.Nodes = new List<Node>();
            CurrentRecordBlock = newBlock;
            return newBlock;
        }

        public static double GetRecordsPerBlock()
        {
            double recordsize = RecordController.GetRecordSize();
            return (BlockSize - BlockAddressSize) / (recordsize + BlockOffsetSize);
        }
    }
}
