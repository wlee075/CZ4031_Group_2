using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    public class Experiment1Controller
    {
        int BlockSize = 100;
        string FileName = "Experiment1Data.txt";
        string Directory = MainController.GetMainDirectory() + "data.tsv";
        public string GetDirectory()
        {
            return Directory;
        }
        public void StoreData()
        {
            AccessFileController afcontroller = new AccessFileController(Directory);
            List<Record> records = afcontroller.ReadAndConvert();
            int maxTconst = records.Select(z => z.Tconst).Max().Count();
            int maxAverageRating = records.Select(z => z.AverageRating.ToString()).Max().Count(); 
            int maxNumVotes = records.Select(z => z.NumVotes.ToString()).Max().Count();
            Console.WriteLine("Max length of tconst: {0}", maxTconst);
            Console.WriteLine("Max length of averageRating: {0}", maxAverageRating);
            Console.WriteLine("Max length of numVotes: {0}", maxNumVotes);
        }
    }
}
