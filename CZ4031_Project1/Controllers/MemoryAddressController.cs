using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    public static class MemoryAddressController
    {
        // Memory Address Byte Indexes 0x[0][1][2][3][4]
        private static byte[] Address { get; set; }
        private static int AddressSize { get; set; }
        private static List<MemoryAddress> MemoryAddresses = new List<MemoryAddress>();
        public static void SetAddressSize(int size)
        {
            Address = new byte[size];
            AddressSize = size;
        }

        private static byte[] GetNewAddress()
        {
            int index = Address.Length - 1;
            Address[index] += 1;
            CheckBytes(index, Address[index]);
            return Address;
        }
        private static void CheckBytes(int index, byte value)
        {
            if(value == 0)
            {
                //increase byte of next index
                Address[index - 1] += 1;
                CheckBytes(index - 1, Address[index - 1]);
            }
        }
        public static void InsertValueIntoMemory(string value)
        {
            byte[] address = new byte[AddressSize];
            GetNewAddress().CopyTo(address, 0);
            MemoryAddress memoryAddress = new MemoryAddress();
            memoryAddress.Address = new byte[AddressSize];
            memoryAddress.Address = address; 
            memoryAddress.Value = value;
            MemoryAddresses.Add(memoryAddress);
        }
        public static List<MemoryAddress> GetAddresses()
        {
            return MemoryAddresses;
        }
    }
}
