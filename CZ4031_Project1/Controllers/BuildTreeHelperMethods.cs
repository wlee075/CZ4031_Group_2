using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    class BuildTreeHelperMethods
    {
        Block root;

        public void search(int key) 
        { 
            //if tree empty
            if (root.Nodes[key] == null)
            {
                Console.WriteLine("Tree is empty. ");
            }
            // traverse and fimd value
            else
            {
                Node cursor = root.Nodes[key];
            }
        }
        public void insert(int key) { }

    }
}
