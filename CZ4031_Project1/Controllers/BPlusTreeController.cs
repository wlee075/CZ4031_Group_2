using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    public static class BPlusTreeController 
    {
        public static BPlusTree BplusTree = new BPlusTree(3);
        //public static int GetMaxKeys()
        //{
        //    decimal maxKeys;
        //    int blockSize = BlockController.BlockSize;
        //    int blockAddressSize = BlockController.BlockAddressSize;
        //    // Get size left for keys and pointers in a node after accounting for node's isLeaf 
        //    Dictionary<byte[], string> memory = MemoryAddressController.GetAddresses();
        //    byte[] pointerSize = memory.Keys.Last();
        //    int keySize = sizeof(int);
        //    maxKeys = Math.Floor(Convert.ToDecimal(blockSize - blockAddressSize - keySize) / (pointerSize.Length + keySize));
        //    return (int)maxKeys;
        //}


    }
}
