using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FibonacciHeap;

namespace FibTreeExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var fibTree = new FibonacciHeap<Int32>();

            var n23 = new FibonacciHeapNode<Int32>(23);
            var n7 = new FibonacciHeapNode<Int32>(7);
            var n21 = new FibonacciHeapNode<Int32>(21);
            var n3 = new FibonacciHeapNode<Int32>(3);
            var n18 = new FibonacciHeapNode<Int32>(18);
            var n39 = new FibonacciHeapNode<Int32>(39);
            n18.AddChild(n39);
            var n52 = new FibonacciHeapNode<Int32>(52);

            var n38 = new FibonacciHeapNode<Int32>(38);
            var n41 = new FibonacciHeapNode<Int32>(41);
            n38.AddChild(n41);

            n3.AddChild(n18);
            n3.AddChild(n52);
            n3.AddChild(n38);


            var n17 = new FibonacciHeapNode<Int32>(17);
            var n30 = new FibonacciHeapNode<Int32>(30);

            n17.AddChild(n30);

            var n24 = new FibonacciHeapNode<Int32>(24);
            var n26 = new FibonacciHeapNode<Int32>(26);
            var n46 = new FibonacciHeapNode<Int32>(46);
            var n35 = new FibonacciHeapNode<Int32>(35);

            n26.AddChild(n35);
            n24.AddChild(n26);
            n24.AddChild(n46);

            fibTree.FibHeapInsert(n23);
            fibTree.FibHeapInsert(n7);
            fibTree.FibHeapInsert(n21);
            fibTree.FibHeapInsert(n3);
            fibTree.FibHeapInsert(n17);
            fibTree.FibHeapInsert(n24);


            var min = fibTree.FibHeapExtractMin();

            var min2 = fibTree.FibHeapExtractMin();

            min2 = fibTree.FibHeapExtractMin();

            min2 = fibTree.FibHeapExtractMin();

            min2 = fibTree.FibHeapExtractMin();

            Console.Write(fibTree.ToString());

            Console.WriteLine("Finished. Press any button to continue");
            Console.ReadLine();
        }
    }
}
