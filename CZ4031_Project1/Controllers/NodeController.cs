using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    public class NodeController
    {
        public static Node createNode(int key, MemoryAddress address)
        {
            Node newNode = new Node();
            newNode.child = null;
            newNode.next = null;
            newNode.nextBlock = null;
            newNode.Address = new List<MemoryAddress>();
            newNode.Key = key;
            newNode.Address.Add(address);
            return newNode;
        }
        public static Node createNode(int key)
        {
            Node newNode = new Node();
            newNode.child = null;
            newNode.next = null;
            newNode.nextBlock = null;
            newNode.Address = new List<MemoryAddress>();
            newNode.Key = key;

            return newNode;
        }

        public static void printNode(Node node)
        {
            Console.WriteLine(node.Key);
        }
    }
}
