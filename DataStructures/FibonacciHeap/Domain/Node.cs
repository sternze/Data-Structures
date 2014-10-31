using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures.Domain
{
    /// <summary>
    /// Implementation of this class is from http://msdn.microsoft.com/en-US/library/ms379572(v=vs.80).aspx
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Node<T>
    {
        // Private member-variables
        private T _element;
        private NodeList<T> neighbors = null;

        public Node() {}
        public Node(T data) : this(data, null) {}
        public Node(T data, NodeList<T> neighbors)
        {
            this._element = data;
            this.neighbors = neighbors;
        }

        public T Value
        {
            get
            {
                return _element;
            }
            set
            {
                _element = value;
            }
        }

        protected NodeList<T> Neighbors
        {
            get
            {
                return neighbors;
            }
            set
            {
                neighbors = value;
            }
        }
    }
}
