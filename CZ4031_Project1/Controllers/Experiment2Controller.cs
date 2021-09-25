using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    public class Experiment2Controller
    {
        public void InsertNode(Block curBlock, int key)
        {
            Block rootBlock = new Block();            
            BuildTreeHelperMethods.GetNumNodes(ref curBlock);
            int numberOfPointers = curBlock.numNodes + 1;
            for (int i = 0; i <= curBlock.numNodes; i++)
            {
                if (key < curBlock.Nodes[i].Key && !(curBlock.ChildBlock[i] is null))
                {
                    InsertNode(curBlock.ChildBlock[i], key);
                    if (curBlock.numNodes == numberOfPointers)
                        BuildTreeHelperMethods.SplitNonLeaf(ref curBlock, ref rootBlock);
                    return;
                }
                else if (key < curBlock.Nodes[i].Key && curBlock.ChildBlock[i] is null)
                {
                    int temp;
                    temp = curBlock.Nodes[i].Key;
                    curBlock.Nodes[i].Key = key;
                    key = temp;

                    if (i == curBlock.numNodes)
                    {
                        curBlock.numNodes++;
                        break;
                    }
                }
            }

            if (curBlock.numNodes == numberOfPointers)
            {
                BuildTreeHelperMethods.SplitLeaf(ref curBlock, ref rootBlock);
            }
        }

    }
}
