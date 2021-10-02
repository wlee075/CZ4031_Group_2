using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    class Experiment3Controller
    {

        public static BPlusTree tree = null;

        public void retrieveMovie()
        {
            tree = Experiment2Controller.tree;

            List<MemoryAddress> list = BPlusTreeController.retrieveMovie(tree, 500);

            decimal total = 0;
            foreach (MemoryAddress r in list)
            {
                total += Convert.ToDecimal(r.Value.Split('-')[2]);

            }
            decimal average = total / list.Count;

            Console.WriteLine("Average Of averageRating: " + average);
            Console.WriteLine("Number of record: " + list.Count);
            //Console.WriteLine("Print 1 memory record: " + Convert.ToString(list.ElementAt(0).Value));
        }
    }
}
