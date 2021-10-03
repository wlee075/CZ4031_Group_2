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
        const int blockAddress = 8;
        double totalRecord = 0;
        int recordSize = 0;
        int availableSpace = blockSize - blockAddress;
        public string GetDirectory()
        {
            return Directory;
        }
        public void StoreData()
        {
            // Read from original dataset
            AccessFileController afController = new AccessFileController(Directory);
            List<Record> records = afController.ReadAndConvertToRecords();

            // Get the minimum and maximum length of each fields
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
            Console.WriteLine("Min length of averageRating: {0}", minAverageRating);
            Console.WriteLine("Max length of averageRating: {0}", maxAverageRating);
            Console.WriteLine("Min length of numVotes: {0}", minNumVotes);
            Console.WriteLine("Max length of numVotes: {0}", maxNumVotes);

            //Set memory address size
            //1 more byte as offsets to store blocks addresses
            MemoryAddressController.SetAddressSize(Convert.ToInt32(GetBlockOffsetSize()) + 1);
            //Sort records by numVotes       
            //records = records.OrderBy(z => z.NumVotes).ToList();
            //Saving records
            Console.WriteLine("Storing records...");
            SaveRecords(maxTconst, maxAverageRating - 1, 4, records);
            Console.WriteLine("Records stored");
            Console.WriteLine("Total number of records: {0}", totalRecord);    
        }
        public void PrintRecords()
        {
            foreach (MemoryAddress memoryaddress in MemoryAddressController.GetAddresses())
            {
                Console.WriteLine("{0}: {1}",BitConverter.ToString(memoryaddress.Address), memoryaddress.Value);
            }
        }
        private void SaveRecords(int tconst, int avgrating, int numvotes, List<Record> records)
        {
            foreach(Record r in records)
            {
                MemoryAddressController.InsertValueIntoMemory(String.Format("{0}-{1}-{2}",r.Tconst , r.NumVotes , r.AverageRating));   
            }
        }
       
     
        public void ShowStatistics()
        {
            if(recordSize!=0)
            {
                decimal blockOffsetSize = Convert.ToDecimal(GetBlockOffsetSize());
                decimal recordsPerBlock = Convert.ToDecimal(availableSpace) / (recordSize + blockOffsetSize);
                recordsPerBlock = Math.Floor(recordsPerBlock);
                decimal blockHeaderSize = blockAddress + blockOffsetSize * recordsPerBlock;
                decimal totalBlocks = Convert.ToDecimal(totalRecord) / recordsPerBlock;
                totalBlocks = Math.Ceiling(totalBlocks);
                decimal sizeOfDatabase = totalBlocks * blockSize;
                Console.WriteLine("Record size: {0} bytes", recordSize);
                Console.WriteLine("Number of records per block: {0}", recordsPerBlock);
                Console.WriteLine("Block address size: {0} bytes", blockAddress);
                Console.WriteLine("Block offset size: {0} bytes", blockOffsetSize);
                Console.WriteLine("Block header size: {0} bytes", blockHeaderSize);
                Console.WriteLine("Total Number of blocks: {0}", totalBlocks);
                Console.WriteLine("Size of Database: {0} bytes", sizeOfDatabase);
            }
            else
            {
                Console.WriteLine("Please store data first.");
            }
        }
        double GetBlockOffsetSize()
        {
            double count = 0;
            double records = totalRecord;
            while(records > 0)
            {
               double highestBit =  Math.Pow(2, count);
                records = records - highestBit;
                count += 1;
            }
            // return bytes
            return Math.Ceiling(count/8);
        }
    }
}
