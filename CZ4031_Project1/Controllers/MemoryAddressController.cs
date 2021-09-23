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

        public static byte[] GetNewAddress(int size)
        {
            if(Address == null)
            {
                AddressSize = 1;
                Address = new byte[AddressSize];
            }

            byte[] address = new byte[AddressSize];
            Address.CopyTo(address, 0);
            Array.Reverse(address);
            AddBytes(0, size);
            return address;
        }
        private static void AddBytes(int index, int size)
        {
            try
            {
                //carry over bytes
                if (Address[index] + size > 255)
                {
                    Address[index] = Convert.ToByte(Address[index] + size - 256);
                    AddBytes(index + 1, 1);
                }
                else
                {
                    Address[index] = Convert.ToByte(Address[index] + size);
                }
            }
            catch
            {
                AddressSize += 1;
                byte[] newAddress = new byte[AddressSize];
                //Console.WriteLine("overflow: {0}", BitConverter.ToString(newAddress));
                //Console.WriteLine(BitConverter.ToString(newAddress));
                Address.CopyTo(newAddress, 0);
                
                Address = newAddress;

                if (Address[index] + size > 255)
                {
                    Address[index] = Convert.ToByte(Address[index] + size - 256);
                    AddBytes(index + 1, 1);
                }
                else
                {
                    Address[index] = Convert.ToByte(Address[index] + size);
                }


            }
        }
        public static byte[] InsertValueIntoMemory(string value, int size, bool isRecord)
        {
            byte[] address = GetNewAddress(size);
            MemoryAddress memoryAddress = new MemoryAddress();
            memoryAddress.Address = new byte[AddressSize];
            memoryAddress.Address = address; 
            memoryAddress.Value = value;
            MemoryAddresses.Add(memoryAddress);
            if (isRecord)
            {
                BlockController.InsertRecordIntoBlock(address);
            }
            Console.WriteLine("{0}---{1}", BitConverter.ToString(address), value);
            return address;
        }
        public static List<MemoryAddress> GetAddresses()
        {
            return MemoryAddresses;
        }
    }
}
