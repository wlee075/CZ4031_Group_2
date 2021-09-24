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
        // key: record's numVotes, value: Block addr
        public void insertRecordIntoTree(Block block, int key)
        {
            Block cursor = block;
            Node currNode = new Node
            {
                Pointer = block.Address
            };
            //if no root exist, create B+ tree root
            if (block.Address == null)
            {
                // create node in MM 
                currNode.Key = key;   
                currNode.IS_LEAF = true; //is both root and leaf
                cursor.Nodes.Add(currNode);
            }
            else
            {
                // traverse nodes to find proper place to insert key
                Node parentNode = new Node();
                parentNode.Pointer = block.Address;
                while(!currNode.IS_LEAF)
                {
                    // set parent node and its address
                    parentNode = currNode;
                    parentNode.Pointer = currNode.Pointer;

                    for(int i = 0; i < cursor.Nodes.Count; i++)
                    {
                        //if key < current key, go to left pointer's node
                        if(key < cursor.Nodes[i].Key)
                        {

                        }
                    }
                }
            }
        }

        

    }
}
