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
        private static List<Block> Blocks = new List<Block>();
        public static void InsertRecordIntoBlock(byte[] pointer)
        {
            //Select block that store records only
            Block block = Blocks.Where(z => z.IsRecord == true).LastOrDefault();
            //If there are no blocks
            if (block == null)
            {
                block = AddBlock();
            }

            //Check if block is full
            int numberOfRecords = block.Nodes.Count();
            if (numberOfRecords >= GetRecordsPerBlock(RecordController.GetRecordSize()))
            {
                block = AddBlock();
            }

            Node node = new Node();
            node.IS_LEAF = true;
            //node.Key = key;
            node.Pointer = pointer;
            block.Nodes.Add(node);

        }
        private static Block AddBlock()
        {
            Block lastBlock = Blocks.Where(z => z.IsRecord == true).LastOrDefault();
            Block block = new Block();
            if (lastBlock == null)
            {
                block.Id = "block1";
            }
            else
            {
                //Increase id by 1
                block.Id = String.Format("block{0}", Convert.ToInt32(lastBlock.Id.Replace("block", "")) + 1);
            }
            block.IsRecord = true;
            block.Address = MemoryAddressController.InsertValueIntoMemory(block.Id, BlockAddressSize, false);
            block.Nodes = new List<Node>();
            Blocks.Add(block);
            return block;
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

        public static List<Block> GetBlocks()
        {
            return Blocks;
        }
        public static double GetRecordsPerBlock(double recordsize)
        {
            return BlockSize - BlockAddressSize / (recordsize + BlockOffsetSize);
        }
    }
}
