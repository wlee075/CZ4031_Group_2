﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Entities
{
    public class Block
    {
        public byte[] Address { get; set; }
        public List<Node> Nodes { get; set; }
    }
}