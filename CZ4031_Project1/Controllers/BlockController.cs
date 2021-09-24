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
        public static void InsertIntoRecordBlock(byte[] pointer)
        {
            //If there are no blocks
            if (CurrentRecordBlock == null)
            {
                CurrentRecordBlock = AddBlock();
            }

            //Check if block is full
            int numberOfRecords = CurrentRecordBlock.Nodes.Count();
            if (numberOfRecords >= GetRecordsPerBlock())
            {
                CurrentRecordBlock = AddBlock();
            }

            Node node = new Node();
            node.IS_LEAF = true;
            //node.Key = key;
            node.Pointer = pointer;
            CurrentRecordBlock.Nodes.Add(node);

        }
        private static Block AddBlock()
        {
            if (CurrentRecordBlock == null)
            {
                CurrentRecordBlock = new Block();
                CurrentRecordBlock.Id = "block1";
            }
            else
            {
                //Increase id by 1
                CurrentRecordBlock.Id = String.Format("block{0}", Convert.ToInt32(CurrentRecordBlock.Id.Replace("block", "")) + 1);
            }
            CurrentRecordBlock.IsRecord = true;
            CurrentRecordBlock.Address = MemoryAddressController.InsertValueIntoMemory(CurrentRecordBlock.Id, BlockAddressSize, false);
            CurrentRecordBlock.Nodes = new List<Node>();
            //Blocks.Add(block);
            return CurrentRecordBlock;
        }
        public static double GetBlockOffsetSize(double recordsize, double totalrecord)
        {
            double count = 0;
            double records = totalrecord;
            double totalrecordsize = recordsize * totalrecord;
            while (totalrecordsize > 0)
            {
                double highestBit = Math.Pow(2, count);
                totalrecordsize = totalrecordsize - highestBit;
                count += 1;
            }
            // return bytes
            BlockOffsetSize = Math.Ceiling(count / 8);
            return BlockOffsetSize;
        }

        //public static List<Block> GetBlocks()
        //{
        //    //return Blocks;
        //}
        public static double GetRecordsPerBlock()
        {
            double recordsize = RecordController.GetRecordSize();
            return (BlockSize - BlockAddressSize) / (recordsize + BlockOffsetSize);
        }
    }
}
