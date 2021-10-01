using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CZ4031_Project1.Controllers;
namespace CZ4031_Project1.Controllers
{
    public class Experiment2Controller
    { 
        public void AAA()
        {
            BPlusTreeController.BplusTree = new BPlusTree(3);
            var bpt = BPlusTreeController.BplusTree;
            bpt.insert(5, 33);
            bpt.insert(15, 21);
            bpt.insert(25, 31);
            bpt.insert(35, 41);
            bpt.insert(45, 10);

            if (bpt.search(15) != 0)
            {
                Console.WriteLine("Found");
            }
            else
            {
                Console.WriteLine("Not Found");
            }
        }
        //BPlusTree bpt = BPlusTreeController.BplusTree;
        //bpt = new BPlusTree(3);
        //bpt.insert(5, 33);
        //bpt.insert(15, 21);
        //bpt.insert(25, 31);
        //bpt.insert(35, 41);
        //bpt.insert(45, 10);
        //System.out.println(bpt.root.childPointers);



    }
}
