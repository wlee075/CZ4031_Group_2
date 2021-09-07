using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    CZ4031_Project1.Retrieve.Start();
                    break;
                case "2":
                    CZ4031_Project1.Insert.Start();
                    break;
                case "3":
                    CZ4031_Project1.Delete.Start();
                    break;
                case "4":

                    break;
                case "5":

                    break;
                default:
                    Console.WriteLine("Invalid selection, please try again.");
                    break;
            }

            Main(null);
        }
    }
}
