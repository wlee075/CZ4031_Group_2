using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Entities
{
    public class BPlusTree
    {
        int m;
        public InternalNode root;
        LeafNode firstLeaf;
        public BPlusTree(int m)
        {
            this.m = m;
            this.root = null;
        }

        public int binarySearch(DictionaryPair[] dps, int numPairs, int t)
        {
            Comparer<DictionaryPair> c = Comparer<DictionaryPair>.Create(binarySearchComparator);
            return Array.BinarySearch(dps, 0, numPairs, new DictionaryPair(t,0), c) ; 
        }
        private int binarySearchComparator(DictionaryPair o1, DictionaryPair o2)
        {
            int a = o1.key;
            int b = o2.key;
            return a.CompareTo(b);
        }
        private void sortDictionary(DictionaryPair[] dictionary)
        {
            Array.Sort(dictionary, sortDictionaryComparator);
        }
        private int sortDictionaryComparator(DictionaryPair o1, DictionaryPair o2)
        {
            if (o1 == null && o2 == null)
            {
                return 0;
            }
            if (o1 == null)
            {
                return 1;
            }
            if (o2 == null)
            {
                return -1;
            }
            return o1.CompareTo(o2);

        }
        public void insert(int key, double value)
        {
            if (isEmpty())
            {

                LeafNode ln = new LeafNode(this.m, new DictionaryPair(key, value));

                this.firstLeaf = ln;

            }
            else
            {
                LeafNode ln = (this.root == null) ? this.firstLeaf : findLeafNode(key);

                if (!ln.insert(new DictionaryPair(key, value)))
                {

                    ln.dictionary[ln.numPairs] = new DictionaryPair(key, value);
                    ln.numPairs++;
                    sortDictionary(ln.dictionary);

                    int midpoint = getMidpoint();
                    DictionaryPair[] halfDict = splitDictionary(ln, midpoint);

                    if (ln.Parent == null)
                    {

                        int[] parent_keys = new int[this.m];
                        parent_keys[0] = halfDict[0].key;
                        InternalNode parent = new InternalNode(this.m, parent_keys);
                        ln.Parent = parent;
                        parent.appendChildPointer(ln);

                    }
                    else
                    {
                        int newParentKey = halfDict[0].key;
                        ln.Parent.keys[ln.Parent.degree - 1] = newParentKey;
                        Array.Sort(ln.Parent.keys, 0, ln.Parent.degree);
                    }

                    LeafNode newLeafNode = new LeafNode(this.m, halfDict, ln.Parent);

                    int pointerIndex = ln.Parent.findIndexOfPointer(ln) + 1;
                    ln.Parent.insertChildPointer(newLeafNode, pointerIndex);

                    newLeafNode.rightSibling = ln.rightSibling;
                    if (newLeafNode.rightSibling != null)
                    {
                        newLeafNode.rightSibling.leftSibling = newLeafNode;
                    }
                    ln.rightSibling = newLeafNode;
                    newLeafNode.leftSibling = ln;

                    if (this.root == null)
                    {

                        this.root = ln.Parent;

                    }
                    else
                    {
                        InternalNode internalNode = ln.Parent;
                        while (internalNode != null) {
                            if (internalNode.isOverfull()) {
                                splitInternalNode(internalNode);
                            } else {
                                break;
                            }
                            internalNode = internalNode.Parent;
                        }
                    }
                }
            }
        }
        private DictionaryPair[] splitDictionary(LeafNode ln, int split)
        {

            DictionaryPair[] dictionary = ln.dictionary;

            DictionaryPair[] halfDict = new DictionaryPair[this.m];

            for (int i = split; i < dictionary.Length; i++)
            {
                halfDict[i - split] = dictionary[i];
                ln.delete(i);
            }

            return halfDict;
        }
        private Node[] splitChildPointers(InternalNode internalNode, int split)
        {

            Node[] pointers = internalNode.childPointers;
            Node[] halfPointers = new Node[this.m + 1];

            for (int i = split + 1; i < pointers.Length; i++)
            {
                halfPointers[i - split - 1] = pointers[i];
                internalNode.removePointer(i);
            }

            return halfPointers;
        }
        private void splitInternalNode(InternalNode internalNode)
        {

            InternalNode parent = internalNode.Parent;

            int midpoint = getMidpoint();
            int newParentKey = internalNode.keys[midpoint];
            int[] halfKeys = splitKeys(internalNode.keys, midpoint);
            Node[] halfPointers = splitChildPointers(internalNode, midpoint);

            internalNode.degree = linearNullSearch(null, internalNode.childPointers);

            InternalNode sibling = new InternalNode(this.m, halfKeys, halfPointers);
            foreach (Node pointer in halfPointers)
            {
                if (pointer != null)
                {
                    pointer.Parent = sibling;
                }
            }

            sibling.rightSibling = internalNode.rightSibling;
            if (sibling.rightSibling != null)
            {
                sibling.rightSibling.leftSibling = sibling;
            }
            internalNode.rightSibling = sibling;
            sibling.leftSibling = internalNode;

            if (parent == null)
            {

                int[] keys = new int[this.m];
                keys[0] = newParentKey;
                InternalNode newRoot = new InternalNode(this.m, keys);
                newRoot.appendChildPointer(internalNode);
                newRoot.appendChildPointer(sibling);
                this.root = newRoot;

                internalNode.Parent = newRoot;
                sibling.Parent = newRoot;

            }
            else
            {

                parent.keys[parent.degree - 1] = newParentKey;
                Array.Sort(parent.keys, 0, parent.degree);

                int pointerIndex = parent.findIndexOfPointer(internalNode) + 1;
                parent.insertChildPointer(sibling, pointerIndex);
                sibling.Parent = parent;
            }
        }
        private int[] splitKeys(int[] keys, int split)
        {

            int[] halfKeys = new int[this.m];

            keys[split] = 0;

            for (int i = split + 1; i < keys.Length; i++)
            {
                halfKeys[i - split - 1] = keys[i];
                keys[i] = 0;
            }

            return halfKeys;
        }
        // Find the leaf node
        public LeafNode findLeafNode(int key)
        {

            int[] keys = this.root.keys;
            int i;

            for (i = 0; i < this.root.degree - 1; i++)
            {
                if (key < keys[i])
                {
                    break;
                }
            }

            Node child = this.root.childPointers[i];
            if (child is LeafNode) {
                return (LeafNode)child;
            } else {
                return findLeafNode((InternalNode)child, key);
            }
        }

        // Find the leaf node
        public LeafNode findLeafNode(InternalNode node, int key)
        {

            int[] keys = node.keys;
            int i;

            for (i = 0; i < node.degree - 1; i++)
            {
                if (key < keys[i])
                {
                    break;
                }
            }
            Node childNode = node.childPointers[i];
            if (childNode is LeafNode) {
                return (LeafNode)childNode;
            } else {
                return findLeafNode((InternalNode)node.childPointers[i], key);
            }
        }
        // Finding the index of the pointer
        public int findIndexOfPointer(Node[] pointers, LeafNode node)
        {
            int i;
            for (i = 0; i < pointers.Length; i++)
            {
                if (pointers[i] == node)
                {
                    break;
                }
            }
            return i;
        }

        // Get the mid point
        public int getMidpoint()
        {
            return (int)Math.Ceiling((this.m + 1) / 2.0) - 1;
        }
        // Balance the tree
        public void handleDeficiency(InternalNode internalNode)
        {

            InternalNode sibling;
            InternalNode parent = internalNode.Parent;

            if (this.root == internalNode)
            {
                for (int i = 0; i < internalNode.childPointers.Length; i++)
                {
                    if (internalNode.childPointers[i] != null)
                    {
                        if (internalNode.childPointers[i] is InternalNode)
                        {
                            this.root = (InternalNode)internalNode.childPointers[i];
                            this.root.Parent = null;
                        }
                        else if (internalNode.childPointers[i] is LeafNode)
                        {
                            this.root = null;
                        }
                    }
                }
            }

            else if (internalNode.leftSibling != null && internalNode.leftSibling.isLendable())
            {
                sibling = internalNode.leftSibling;
            }
            else if (internalNode.rightSibling != null && internalNode.rightSibling.isLendable())
            {
                sibling = internalNode.rightSibling;

                int borrowedKey = sibling.keys[0];
                Node pointer = sibling.childPointers[0];

                internalNode.keys[internalNode.degree - 1] = parent.keys[0];
                internalNode.childPointers[internalNode.degree] = pointer;

                parent.keys[0] = borrowedKey;

                sibling.removePointer(0);
                Array.Sort(sibling.keys);
                sibling.removePointer(0);
                ShiftDown(internalNode.childPointers, 1);
            }
            else if (internalNode.leftSibling != null && internalNode.isMergeable())
            {

            }
            else if (internalNode.rightSibling != null && internalNode.rightSibling.isMergeable())
            {
                sibling = internalNode.rightSibling;
                sibling.keys[sibling.degree - 1] = parent.keys[parent.degree - 2];
                Array.Sort(sibling.keys, 0, sibling.degree);
                parent.keys[parent.degree - 2] = 0;

                for (int i = 0; i < internalNode.childPointers.Length; i++)
                {
                    if (internalNode.childPointers[i] != null)
                    {
                        sibling.prependChildPointer(internalNode.childPointers[i]);
                        internalNode.childPointers[i].Parent = sibling;
                        internalNode.removePointer(i);
                    }
                }

                parent.removePointer(internalNode);

                sibling.leftSibling = internalNode.leftSibling;
            }

            if (parent != null && parent.isDeficient())
            {
                handleDeficiency(parent);
            }
        }


        public Double search(int key)
        {

            if (isEmpty())
            {
                return 0;
            }

            LeafNode ln = (this.root == null) ? this.firstLeaf : findLeafNode(key);

            DictionaryPair[] dps = ln.dictionary;
            int index = binarySearch(dps, ln.numPairs, key);

            if (index < 0)
            {
                return 0;
            }
            else
            {
                return dps[index].value;
            }
        }

        public List<double> search(int lowerBound, int upperBound)
        {

            List<double> values = new List<double>();

            LeafNode currNode = this.firstLeaf;
            while (currNode != null)
            {

                DictionaryPair[] dps = currNode.dictionary;
                foreach (DictionaryPair dp in dps)
                {

                    if (dp == null)
                    {
                        break;
                    }

                    if (lowerBound <= dp.key && dp.key <= upperBound)
                    {
                        values.Add(dp.value);
                    }
                }
                currNode = currNode.rightSibling;

            }

            return values;
        }
        public bool isEmpty()
        {
            return firstLeaf == null;
        }

        public int linearNullSearch(DictionaryPair[] dps, Node[] nodes)
        {
            if (nodes != null)
            {
                for (int i = 0; i < nodes.Length; i++)
                {
                    if (nodes[i] == null)
                    {
                        return i;
                    }
                }
            }
            if (dps != null)
            {
                for (int i = 0; i < dps.Length; i++)
                {
                    if (dps[i] == null)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        public void ShiftDown(Node[] pointers, int amount)
        {
            Node[] newPointers = new Node[m + 1];
            for (int i = amount; i < pointers.Length; i++)
            {
                newPointers[i - amount] = pointers[i];
            }
            pointers = newPointers;
        }
    }
}
