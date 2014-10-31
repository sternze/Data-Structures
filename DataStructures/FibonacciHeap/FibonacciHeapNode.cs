using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciHeap
{
    public class FibonacciHeapNode<T>
    {

        private T _element;
        private FibonacciHeapNode<T> _parent;
        private FibonacciHeapNode<T> _child;
        private FibonacciHeapNode<T> _left;
        private FibonacciHeapNode<T> _right;
        private int _degree;
        private bool _marked;
        private int _subNodes;

        public FibonacciHeapNode(T element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element", "The element of a Fibonacci Node Object may not be null");
            }

            this._element = element;
            this._child = null;
            this._parent = null;
            this._left = this;
            this._right = this;
            this._degree = 0;
            this._marked = false;
            this._subNodes = 0;
        }

        public FibonacciHeapNode<T> Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public FibonacciHeapNode<T> Child
        {
            get { return _child; }
            set { _child = value; }
        }

        public FibonacciHeapNode<T> Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public FibonacciHeapNode<T> Right
        {
            get { return _right; }
            set { _right = value; }
        }

        public int Degree
        {
            get { return _degree; }
            set { _degree = value; }
        }

        public bool Marked
        {
            get { return _marked; }
            set { _marked = value; }
        }

        public T Element
        {
            get { return _element; }
            set { _element = value; }
        }

        public int SubNodes
        {
            get { return _subNodes; }
            set { _subNodes = value; }
        }

        /// <summary>
        /// Adds a child to the current node.
        /// 
        /// The degree of the current node will be increased, because there will be one node more.
        /// </summary>
        /// <param name="nodeToAdd">The node which should be added.</param>
        public void AddChild(FibonacciHeapNode<T> nodeToAdd)
        {
            if (this.Child != null)
            {
                nodeToAdd.Left = this.Child;
                nodeToAdd.Right = this.Child.Right;

                nodeToAdd.Right.Left = nodeToAdd;
                this.Child.Right = nodeToAdd;

            }
            else
            {
                this.Child = nodeToAdd;
                nodeToAdd.Left = this.Child;
                nodeToAdd.Right = this.Child;
            }

            nodeToAdd.Parent = this;

            this._degree++;
            this.SubNodes += 1 + nodeToAdd.SubNodes;
        }

        /// <summary>
        /// This method removes a child from the current node.
        /// No need to check if the node is really a child node, because the only call
        /// is from Method "Cut", which will - by definition - act on childs and parents.
        /// 
        /// The degree of the current node is decreased, because there will be one node less.
        /// </summary>
        /// <param name="nodeToRemove">The child to be removed</param>
        public void RemoveChild(FibonacciHeapNode<T> nodeToRemove)
        {
            
            var leftNeighbour = nodeToRemove.Left;
            var rightNeighbour = nodeToRemove.Right;

            leftNeighbour.Right = rightNeighbour;
            rightNeighbour.Left = leftNeighbour;

            nodeToRemove.Left = nodeToRemove;
            nodeToRemove.Right = nodeToRemove;

            this.SubNodes -= (1 + nodeToRemove.SubNodes);
            this._degree--;
        }
    }
}
