using CZ4031_Project1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Views
{
    public static class Experiment1View
    {
        static Experiment1Controller controller;
        public static void Display()
        {
            controller = new Experiment1Controller();
            string directory = controller.GetDirectory();
            Console.WriteLine("Please key in your selection: ");
            Console.WriteLine("1. Store the data from {0}", directory);
            Console.WriteLine("2. Show statistics");
            Console.WriteLine("3. Back to main page");
            Console.WriteLine("4. Exit");

           
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    DisplayStoreView();
                    break;
                case "2":
                    DisplayStatisticsView();
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

        static void DisplayStoreView()
        {
            controller.StoreData();
        }
        static void DisplayStatisticsView()
        {

        }
    }
}
