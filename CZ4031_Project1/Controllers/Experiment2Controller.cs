using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    public class Experiment2Controller
    {
        public static BPlusTree tree = null;
        public void BuildTree()
        {
            // Read from original dataset
            tree = BPlusTreeController.createTree(300);
            List<MemoryAddress> addresses = MemoryAddressController.GetAddresses();
            /*
            int[] test = { 1, 4, 7, 10, 17, 21, 31, 25, 19, 20, 28,42};
            foreach (int t in test){
                BPlusTreeController.insert(tree, t, null);

            }
            Console.WriteLine("n: " + tree.getN());
            Console.WriteLine("Number of Nodes: " + tree.getNumberOfNodes());
            Console.WriteLine("Number of Internal Nodes: "+ tree.getNumberOfInternalNodes());
            Console.WriteLine("Tree Height: " + tree.getHeight());
            Console.Write("Content of Root Node: ");
            BlockController.printBlock(tree.rootBlock);
            Console.Write("Content of 1st child Node: ");
            BlockController.printBlock(tree.rootBlock.child);
            
            BPlusTreeController.printTree(tree);
            */
            foreach (MemoryAddress x in addresses){
                
                int recordToBeInserted = Convert.ToInt32(x.Value.Split('-')[1]);
                BPlusTreeController.insert(tree, recordToBeInserted, x);
                
            }
            //Console.WriteLine("n: " + tree.getN());
            //Console.WriteLine("Number of Nodes: " + tree.getNumberOfNodes());
            //Console.WriteLine("Number of Internal Nodes: " + tree.getNumberOfInternalNodes());
            //Console.WriteLine("Tree Height: " + tree.getHeight());
            //Console.Write("Content of Root Node: ");
            //BlockController.printBlock(tree.rootBlock);
            //Console.Write("Content of 1st child Node: ");
            //BlockController.printBlock(tree.rootBlock.child);
            //List<MemoryAddress> list = BPlusTreeController.retrieveMovie(tree, 500);
            //decimal total = 0;
            //foreach (MemoryAddress r in list)
            //{
            //    total += Convert.ToDecimal(r.Value.Split('-')[2]);
            //}
            //decimal average = total / list.Count;

            //Console.WriteLine("Average Of averageRating: "+ average);
            //Console.WriteLine("Number of record: " + list.Count);
        }
    }
}
