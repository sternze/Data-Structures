using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FibonacciHeap;

namespace FibonacciHeap
{
    public class FibonacciHeap<T> : IDisposable
    {
        private FibonacciHeapNode<T> _minimumNode;
        private int _numberOfElements;
        private readonly HashSet<FibonacciHeapNode<T>> _rootElements;
        private readonly IFibonacciHeapComparer<T> _comparer;
        private readonly double GOLDEN_RATIO = (1 + Math.Sqrt(5))/2;

        // Flag: Has Dispose already been called? 
        private bool _disposed = false;

        public FibonacciHeap() : this(null)
        {
        }

        public FibonacciHeap(IFibonacciHeapComparer<T> comparer)
        {
            this._minimumNode = null;
            this._numberOfElements = 0;
            this._rootElements = new HashSet<FibonacciHeapNode<T>>();
            this._comparer = comparer;
        }

        /// <summary>
        /// Inserts a FibonacciHeapNode into the Fibonacci-Tree.
        /// </summary>
        /// <param name="nodeToInsert">The node that should be inserted.</param>
        public void FibHeapInsert(FibonacciHeapNode<T> nodeToInsert)
        {
            if (nodeToInsert == null)
            {
                throw new ArgumentNullException("nodeToInsert", "The node to be inserted in the Fibonacci Heap may not be null!");
            }

//            nodeToInsert.Degree = 0;
//            nodeToInsert.Child = null;
//            nodeToInsert.Marked = false;

            InsertInRootElements(nodeToInsert);
            

            this._numberOfElements += 1 + nodeToInsert.SubNodes;
        }

        /// <summary>
        /// Returns the minimum Object in the current Fibonacci Heap
        /// </summary>
        /// <returns>The minimum Object in the current Fibonacci Heap</returns>
        public FibonacciHeapNode<T> FibHeapGetMin()
        {
            return _minimumNode;
        }

        /// <summary>
        /// Unions the two Heaps and destroys h2.
        /// </summary>
        /// <param name="h">A Heap that should be united with the current heap</param>
        public void FibHeapUnion(FibonacciHeap<T> h)
        {
            foreach (var nodeToInsert in h._rootElements)
            {
                InsertInRootElements(nodeToInsert);
            }

            this._numberOfElements += h._numberOfElements;

            h.Dispose();
        }

        /// <summary>
        /// Deletes a node from the FibonacciHeap
        /// </summary>
        /// <param name="nodeToDelete">The node that should be deleted.</param>
        public void FibHeapDelete(FibonacciHeapNode<T> nodeToDelete)
        {
            FibHeapDecreaseKey(nodeToDelete, _comparer.GetSmallestPossibleElement());
            FibHeapExtractMin();
        }

        /// <summary>
        /// Inserts a Node into the rootElements List and adjusts the double linked list.
        /// </summary>
        /// <param name="nodeToInsert">The Node that should be inserted into the root list</param>
        /// <param name="updateMinimum">A flag which describes weather or not to update the Minimum.</param>
        private void InsertInRootElements(FibonacciHeapNode<T> nodeToInsert, bool updateMinimum = true)
        {
            // Insert the node into the double linked list.
            if (this._minimumNode != null)
            {
                nodeToInsert.Left = this._minimumNode;
                nodeToInsert.Right = this._minimumNode.Right;
                nodeToInsert.Right.Left = nodeToInsert;
                this._minimumNode.Right = nodeToInsert;
            }
            else
            {
                nodeToInsert.Right = nodeToInsert;
                nodeToInsert.Left = nodeToInsert;
            }

            nodeToInsert.Parent = null;
            this._rootElements.Add(nodeToInsert);
            
            // if minimumNode is greater than nodeToInsert or the minimumNode is null, nodeToInsert is the new minimumNode
            if (updateMinimum && (this._minimumNode == null || Compare(_minimumNode, nodeToInsert) > 0))
            {
                this._minimumNode = nodeToInsert;
            }
        }

        /// <summary>
        /// Helper method, which compares two FibonacciHeapNode objects
        /// </summary>
        /// <param name="x">First object</param>
        /// <param name="y">Second object</param>
        /// <returns>A negative number if x is smaller than y, a positive number if x is greater than y, 0 if they are equal</returns>
        private int Compare(FibonacciHeapNode<T> x, FibonacciHeapNode<T> y)
        {
            if (this._comparer != null)
            {
                return this._comparer.Compare(x.Element, y.Element);
            }
            var xComparable = (IComparable<T>)x.Element;
            return xComparable.CompareTo(y.Element);
        }

        /// <summary>
        /// This method removes the Node with the smallest value from the Fibonacci Heap
        /// </summary>
        /// <returns>The removed Node</returns>
        public FibonacciHeapNode<T> FibHeapExtractMin()
        {
            var min = this._minimumNode;

            if (min == null || (min.Child == null)) return min;


            // Add all child nodes of the current minimum to the root list.
            var childOfMin = min.Child;

            var currentChild = childOfMin;

            do
            {
                var nextElement = currentChild.Right;
                InsertInRootElements(currentChild, false);

                currentChild = nextElement;
            } while (!currentChild.Equals(childOfMin));

            this._rootElements.Remove(min);

            if (min == min.Right)
            {
                this._minimumNode = null;
            }
            else
            {
                this._minimumNode = min.Right;
                Consolidate();
            }
                
            this._numberOfElements--;

            return min;
        }

        /// <summary>
        /// Consolidates the Fibonacci Heap according to the specified algorithm.
        /// Reorganizes sub-trees, etc.
        /// </summary>
        private void Consolidate()
        {
            var upperBound = UpperBound();
            var aux = new FibonacciHeapNode<T>[upperBound];

            for (int i = 0; i < upperBound; i++)
            {
                aux[i] = null;
            }

            var numElementsInRootList = _rootElements.Count;

            for (int i = 0; i < numElementsInRootList; i++)
            {
                var node = _rootElements.ElementAt(i);
                var nodeDegree = node.Degree;

                while (aux[nodeDegree] != null)
                {
                    var secondNode = aux[nodeDegree]; //another node with the same degree

                    if (secondNode != null)
                    {
                        if (Compare(node, secondNode) > 0)
                        {
                            var tmp = node;
                            node = secondNode;
                            secondNode = tmp;
                            tmp = null;
                            i--;            // need to decrease i here, because the current element was deleted, from _rootElements, and I want to get the 
                                            // next element which is now at position i
                        }

                        FibHeapLink(secondNode, node);
                        numElementsInRootList = _rootElements.Count;
                        aux[nodeDegree] = null;
                        nodeDegree++;
                    }
                }

                aux[nodeDegree] = node;
            }

            this._minimumNode = null;

            for (int i = 0; i < upperBound; i++)
            {
                if (aux[i] != null)
                {
                    if (this._minimumNode == null)
                    {
                        this._rootElements.Clear();
                    }

                    InsertInRootElements(aux[i]);
                }
            }

        }

        /// <summary>
        /// Removes a node from the Root-Element list and adds it as a child.
        /// </summary>
        /// <param name="futureParentNode">The node that will become the parent.</param>
        /// <param name="node">The node that will be removed from the Root-Element set, and will be a child in the future.</param>
        private void FibHeapLink(FibonacciHeapNode<T> futureParentNode, FibonacciHeapNode<T> node)
        {
            this._rootElements.Remove(futureParentNode);

            node.AddChild(futureParentNode);
            futureParentNode.Marked = false;
        }

        /// <summary>
        /// Calculates the Upperbound of the auxilliary-Array used in <see cref="Consolidate"/>
        /// </summary>
        /// <returns>The upper bound of the auxilliary-Array</returns>
        private int UpperBound()
        {
            return (int)Math.Log(this._numberOfElements, GOLDEN_RATIO);
        }

        /// <summary>
        /// Decreases the key of a given node.
        /// </summary>
        /// <param name="node">The node which key should be decreased.</param>
        /// <param name="key">The decreased key.</param>
        public void FibHeapDecreaseKey(FibonacciHeapNode<T> node, T key)
        {
            if (Compare(new FibonacciHeapNode<T>(key), node) > 0)
            {
                throw new ArgumentException("The supplied Key is bigger than the actual one");
            }

            node.Element = key;

            var currentParent = node.Parent;

            if (currentParent != null && Compare(node, currentParent) < 0)
            {
                Cut(node, currentParent);
                CascadingCut(currentParent);
            }

            if (Compare(node, this._minimumNode) < 0)
            {
                this._minimumNode = node;
            }

        }

        /// <summary>
        /// Helper Method that pushes a child node to the Root-Elements list.
        /// </summary>
        /// <param name="node">The node which will be pushed</param>
        /// <param name="currentParent">The current parent of the Node that will be pushed.</param>
        private void Cut(FibonacciHeapNode<T> node, FibonacciHeapNode<T> currentParent)
        {
            currentParent.RemoveChild(node);
            InsertInRootElements(node);
            node.Marked = false;
        }

        /// <summary>
        /// Recursive Helper Method, that marks nodes, and pushes them to the Root-Element list.
        /// </summary>
        /// <param name="node">The node that should be checked.</param>
        private void CascadingCut(FibonacciHeapNode<T> node)
        {
            var parent = node.Parent;

            if (parent != null)
            {
                if (node.Marked == false)
                {
                    node.Marked = true;
                }
                else
                {
                    Cut(node, parent);
                    CascadingCut(parent);
                }
            }
        }

        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                this._rootElements.Clear();
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }

        public override string ToString()
        {
            string retVal = "Your Tree is empty.";
            if (this._minimumNode != null)
            {
                var currentElement = this._minimumNode;

                retVal = PrintLineOfNodeAndBelow(currentElement);

            }
            return retVal;
        }

        private string PrintLineOfNodeAndBelow(FibonacciHeapNode<T> element)
        {
            if (element != null)
            {
                string retVal = String.Empty;
                var currentElement = element;


                do
                {
                    retVal += currentElement.Element.ToString() + ":" + (currentElement.Parent != null
                        ? currentElement.Parent.Element.ToString()
                        : "");

                    retVal += "\n";
                    retVal += PrintLineOfNodeAndBelow(currentElement.Child);

                    retVal += "\n\n";
                    currentElement = currentElement.Right;
                } while (!currentElement.Equals(element));

                return retVal;
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
