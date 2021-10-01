using CZ4031_Project1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Entities
{
    public class LeafNode : Node
    {
        public int maxNumPairs;
        public int minNumPairs;
        public int numPairs;
        public LeafNode leftSibling;
        public LeafNode rightSibling;
        public DictionaryPair[] dictionary;

        public void delete(int index)
        {
            this.dictionary[index] = null;
            numPairs--;
        }

        public bool insert(DictionaryPair dp)
        {
            if (this.isFull())
            {
                return false;
            }
            else
            {
                this.dictionary[numPairs] = dp;
                numPairs++;
                Array.Sort(this.dictionary);

                return true;
            }
        }

        public bool isDeficient()
        {
            return numPairs < minNumPairs;
        }

        public bool isFull()
        {
            return numPairs == maxNumPairs;
        }

        public bool isLendable()
        {
            return numPairs > minNumPairs;
        }

        public bool isMergeable()
        {
            return numPairs == minNumPairs;
        }

        public LeafNode(int m, DictionaryPair dp)
        {
            this.maxNumPairs = m - 1;
            this.minNumPairs = (int)(Math.Ceiling((double)m / 2) - 1);
            this.dictionary = new DictionaryPair[m];
            this.numPairs = 0;
            this.insert(dp);
        }

        public LeafNode(int m, DictionaryPair[] dps, InternalNode parent)
        {
            this.maxNumPairs = m - 1;
            this.minNumPairs = (int)(Math.Ceiling((double)m / 2) - 1);
            this.dictionary = dps;
            this.numPairs = BPlusTreeController.BplusTree.linearNullSearch(dps, null);
            this.Parent = parent;
        }
    }
}
