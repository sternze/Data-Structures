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
    public class GraphVertex<T> : Node<T>
    {
        private List<int> costs;

        public GraphVertex() : base() { }
        public GraphVertex(T value) : base(value) { }
        public GraphVertex(T value, NodeList<T> neighbors) : base(value, neighbors) { }

        new public NodeList<T> Neighbors
        {
            get
            {
                if (base.Neighbors == null)
                    base.Neighbors = new NodeList<T>();

                return base.Neighbors;
            }
        }

        public List<int> Costs
        {
            get
            {
                if (costs == null)
                    costs = new List<int>();

                return costs;
            }
        }
    }
}
