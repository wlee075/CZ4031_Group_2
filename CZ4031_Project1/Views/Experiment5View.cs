using CZ4031_Project1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Views
{
    public static class Experiment5View
    {
        public static void Display()
        {
            Experiment1Controller exp1 = new Experiment1Controller();
            exp1.StoreData();
            Experiment2Controller exp2 = new Experiment2Controller();
            exp2.BuildTree();

            BPlusTreeController.DeleteNode(1000);
            Console.WriteLine("Number of times node is deleted: " + BPlusTreeController.DeletedAmount);
            Console.WriteLine("Number of Nodes: " + Experiment2Controller.tree.getNumberOfNodes());
            Console.WriteLine("Tree Height: " + Experiment2Controller.tree.getHeight());
            Console.Write("Content of Root Node: ");
            BlockController.printBlock(Experiment2Controller.tree.rootBlock);
            Console.Write("Content of 1st child Node: ");
            BlockController.printBlock(Experiment2Controller.tree.rootBlock.child);

        }
    }
}
