using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    public static class BlockController
    {
        private static List<Block> Blocks { get; set; }
        public static List<Block> GetAddresses()
        {
            return Blocks;
        }
    }
}
