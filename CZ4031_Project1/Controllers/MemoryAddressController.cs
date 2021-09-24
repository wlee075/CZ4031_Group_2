﻿using CZ4031_Project1.Entities;
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
        private static Dictionary<byte[], string> MemoryAddresses = new Dictionary<byte[], string>();

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

            MemoryAddresses[address] = value;
            if (isRecord)
            {
                BlockController.InsertIntoRecordBlock(address);
            }
           // Console.WriteLine("{0}---{1}", BitConverter.ToString(address), MemoryAddresses[address]);
            return address;
        }
        public static Dictionary<byte[],string> GetAddresses()
        {
            return MemoryAddresses;
        }
    }
}
