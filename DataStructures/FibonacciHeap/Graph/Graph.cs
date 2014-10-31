using DataStructures.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures.Graph
{
    /// <summary>
    /// Implementation of this class is from http://msdn.microsoft.com/en-US/library/ms379574(v=vs.80).aspx
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Graph<T> : IEnumerable<T>
    {
        private NodeList<T> nodeSet;

        public Graph() : this(null) { }
        public Graph(NodeList<T> nodeSet)
        {
            if (nodeSet == null)
                this.nodeSet = new NodeList<T>();
            else
                this.nodeSet = nodeSet;
        }

        public void AddNode(GraphVertex<T> node)
        {
            // adds a node to the graph
            nodeSet.Add(node);
        }

        public void AddNode(T value)
        {
            // adds a node to the graph
            nodeSet.Add(new GraphVertex<T>(value));
        }

        public void AddDirectedEdge(GraphVertex<T> from, GraphVertex<T> to, int cost)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);
        }

        public void AddUndirectedEdge(GraphVertex<T> from, GraphVertex<T> to, int cost)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);

            to.Neighbors.Add(from);
            to.Costs.Add(cost);
        }

        public bool Contains(T value)
        {
            return nodeSet.FindByValue(value) != null;
        }

        public bool Remove(T value)
        {
            // first remove the node from the nodeset
            GraphVertex<T> nodeToRemove = (GraphVertex<T>)nodeSet.FindByValue(value);
            if (nodeToRemove == null)
                // node wasn't found
                return false;

            // otherwise, the node was found
            nodeSet.Remove(nodeToRemove);

            // enumerate through each node in the nodeSet, removing edges to this node
            foreach (GraphVertex<T> gnode in nodeSet)
            {
                int index = gnode.Neighbors.IndexOf(nodeToRemove);
                if (index != -1)
                {
                    // remove the reference to the node and associated cost
                    gnode.Neighbors.RemoveAt(index);
                    gnode.Costs.RemoveAt(index);
                }
            }

            return true;
        }

        public NodeList<T> Nodes
        {
            get
            {
                return nodeSet;
            }
        }

        public int Count
        {
            get { return nodeSet.Count; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in this.Nodes)
            {
                if (item != null && item.Value != null)
                {
                    yield return item.Value;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (var item in this.Nodes)
            {
                if (item != null && item.Value != null)
                {
                    yield return item.Value;
                }
            }
        }
    }
}
