using CZ4031_Project1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Views
{
    public static class Experiment2View
    {
        static Experiment2Controller controller = new Experiment2Controller();
        public static void Display()
        {
            Console.WriteLine("Please key in your selection: ");
            Console.WriteLine("1. Build the B+ Tree by numVotes");
            Console.WriteLine("2. Show statistics");
            Console.WriteLine("3. Back to main page");
            Console.WriteLine("4. Exit");


            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                   // controller.BuildBpTree();
                    break;
                case "2":
                    
                    break;
                case "3":
                    Views.MainView.Display();
                    break;
                case "4":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid selection, please try again.");
                    Display();
                    break;
            }
            Display();
        }
    }
}
