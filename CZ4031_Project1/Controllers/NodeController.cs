using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    public class NodeController
    {
        public Node FindNode(string Ptr, List<Node> Nodes)
        {
            Node n = Nodes.Where(z => z.Key == Ptr).FirstOrDefault();
            return n;
        }
        public List<Node> FindNodesInBucket(Node N, List<Node> Nodes)
        {
            List<Node> nodes = Nodes.Where(z => z.Bucket == N.Bucket).ToList();
            return nodes;
        }
    }
}
