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
        string Directory = MainController.GetMainDirectory() + "data.tsv";
        const int blockSize = 100;
        const int blockHeader = 10;
        int totalRecord = 0;
        int recordSize = 0;
        int recordPtrSize = 3;
        int availableSpace = blockSize - blockHeader;
        public string GetDirectory()
        {
            return Directory;
        }
        public void StoreData()
        {
            // Read from original dataset
            AccessFileController afController = new AccessFileController(Directory);
            List<Record> records = afController.ReadAndConvertToRecords();

            // Get the maximum length of each fields
            int minTconst = records.Select(z => z.Tconst).Min().Count();
            int maxTconst = records.Select(z => z.Tconst).Max().Count();
            int minAverageRating = records.Select(z => z.AverageRating.ToString()).Min().Count();
            int maxAverageRating = records.Select(z => z.AverageRating.ToString()).Max().Count();
            int minNumVotes = records.Select(z => z.NumVotes.ToString()).Min().Count();
            int maxNumVotes = records.Select(z => z.NumVotes.ToString()).Max().Count();
            totalRecord = records.Count();
            recordSize = maxTconst + maxAverageRating - 1 + 4;
            Console.WriteLine("Min length of tconst: {0}", minTconst);
            Console.WriteLine("Max length of tconst: {0}", maxTconst);
            Console.WriteLine("Max length of averageRating: {0}", minAverageRating);
            Console.WriteLine("Max length of averageRating: {0}", maxAverageRating);
            Console.WriteLine("Max length of numVotes: {0}", minNumVotes);
            Console.WriteLine("Max length of numVotes: {0}", maxNumVotes);
            Console.WriteLine("Total number of records: {0}", totalRecord);
        }
        public void ShowStatistics()
        {
            if(recordSize!=0)
            {
                int recordsPerBlock = (blockSize - blockHeader) / (recordSize + recordPtrSize);
                decimal totalBlocks = Convert.ToDecimal(totalRecord) / (recordSize * recordsPerBlock);
                totalBlocks = Math.Ceiling(totalBlocks);
                decimal sizeOfDatabase = totalBlocks * blockSize;
                Console.WriteLine("Record size: {0}", recordSize);
                Console.WriteLine("Number of records per block: {0}", recordsPerBlock);
                Console.WriteLine("Total Number of blocks: {0}", totalBlocks);
                Console.WriteLine("Size of Database: {0}", sizeOfDatabase);
            }
            else
            {
                Console.WriteLine("Please store data first.");
            }
        }
    }
}
