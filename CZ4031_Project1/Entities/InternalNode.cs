using CZ4031_Project1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Entities
{
    public class InternalNode : Node
    {
        public int maxDegree;
        public int minDegree;
        public int degree;
        public InternalNode leftSibling;
        public InternalNode rightSibling;
        public int[] keys;
        public Node[] childPointers;

        public void appendChildPointer(Node pointer)
        {
            this.childPointers[degree] = pointer;
            this.degree++;
        }

        public int findIndexOfPointer(Node pointer)
        {
            for (int i = 0; i < childPointers.Length; i++)
            {
                if (childPointers[i] == pointer)
                {
                    return i;
                }
            }
            return -1;
        }

        public void insertChildPointer(Node pointer, int index)
        {
            for (int i = degree - 1; i >= index; i--)
            {
                childPointers[i + 1] = childPointers[i];
            }
            this.childPointers[index] = pointer;
            this.degree++;
        }

        public bool isDeficient()
        {
            return this.degree < this.minDegree;
        }

        public bool isLendable()
        {
            return this.degree > this.minDegree;
        }

        public bool isMergeable()
        {
            return this.degree == this.minDegree;
        }

        public bool isOverfull()
        {
            return this.degree == maxDegree + 1;
        }

        public void prependChildPointer(Node pointer)
        {
            for (int i = degree - 1; i >= 0; i--)
            {
                childPointers[i + 1] = childPointers[i];
            }
            this.childPointers[0] = pointer;
            this.degree++;
        }

        public void removeKey(int index)
        {
            this.keys[index] = 0;
        }

        public void removePointer(int index)
        {
            this.childPointers[index] = null;
            this.degree--;
        }

        public void removePointer(Node pointer)
        {
            for (int i = 0; i < childPointers.Length; i++)
            {
                if (childPointers[i] == pointer)
                {
                    this.childPointers[i] = null;
                }
            }
            this.degree--;
        }

        public InternalNode(int m, int[] keys)
        {
            this.maxDegree = m;
            this.minDegree = (int)Math.Ceiling(m / 2.0);
            this.degree = 0;
            this.keys = keys;
            this.childPointers = new Node[this.maxDegree + 1];
        }

        public InternalNode(int m, int[] keys, Node[] pointers)
        {
            this.maxDegree = m;
            this.minDegree = (int)Math.Ceiling(m / 2.0);
            this.degree = BPlusTreeController.BplusTree.linearNullSearch(null, pointers);
            this.keys = keys;
            this.childPointers = pointers;
        }
    }
}
