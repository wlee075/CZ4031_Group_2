﻿using CZ4031_Project1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Views
{
    public static class Experiment4View
    {
        static Experiment4Controller controller = new Experiment4Controller();
        public static void Display()
        {
            Console.WriteLine("Please key in your selection: ");
            Console.WriteLine("1. Retrieve movies numVote 30,000 to 40,000");
            Console.WriteLine("2. Back to main page");
            Console.WriteLine("3. Exit");


            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    controller.retrieveMovie();
                    break;
                case "2":
                    Views.MainView.Display();
                    break;
                case "3":
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
