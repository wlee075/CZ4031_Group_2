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
        public void BuildBpTree()
        {
            // Read from original dataset
            string fileName = "data.tsv";
            string directory = MainController.GetMainDirectory() +  fileName;
            AccessFileController afController = new AccessFileController(directory);

            // Sort by numVotes
            List<Record> records = afController.ReadAndConvertToRecords();
            records = records.OrderBy(z => z.NumVotes).ToList();

            // Write memory to physical address
            directory = MainController.GetMainDirectory();
            
            int count = 1;
            while (records.Count() > 0)
            {
                fileName = MainController.GetMainDirectory() + String.Format("experiment2\\block{0}.txt", count);
                List<string> lines = new List<string>();
                List<Record> tmpRecords = records.Take(5).ToList();
                foreach (Record r in tmpRecords)
                {
                    string line = String.Format("{0}#{1}#{2}", r.NumVotes, r.Tconst, r.AverageRating);
                    lines.Add(line);
                    records.Remove(records.Where(z=> r.Tconst == z.Tconst).First());                   
                }
                afController = new AccessFileController(fileName);
                afController.Write(lines);
                count += 1;
            }
        }
    }
}
