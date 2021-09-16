using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZ4031_Project1.Controllers;
namespace CZ4031_Project1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please key in your selection: ");
            Console.WriteLine("1. Retrieve");
            Console.WriteLine("2. Insert");
            Console.WriteLine("3. Delete");
            Console.WriteLine("4. ");
            Console.WriteLine("5. Exit");

            MainController mc = new MainController();
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    mc.Retrieve();
                    AccessFileController f = new AccessFileController("C:\\CZ4031_Project1_Grp2\\Test.txt");
                    f.Write();
                    break;
                case "2":
                    mc.Insert();
                    break;
                case "3":
                    mc.Delete();
                    break;
                case "4":

                    break;
                case "5":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid selection, please try again.");
                    break;
            }

            Main(args);
        }
    }
}
