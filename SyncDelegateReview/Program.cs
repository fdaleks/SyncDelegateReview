using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncDelegateReview
{
    public delegate int BinaryOp(int x, int y);

    class Program
    {
        private static bool isDone = false;

        static void Main(string[] args)
        {
            //Console.WriteLine($"");

            Console.WriteLine($"***** Delegate Review *****");

            Console.WriteLine($"'Main()' invoked on thread {Thread.CurrentThread.ManagedThreadId}");
            BinaryOp binaryOp = new BinaryOp(Add);

            string text = "'Main()' thanks you for adding these numbers";
            IAsyncResult ar = binaryOp.BeginInvoke(40, 2, new AsyncCallback(AddComplete), text);
            
            while(!isDone)
            {
                Console.WriteLine($"Working at thread {Thread.CurrentThread.ManagedThreadId}...");
                Thread.Sleep(1000);
            }
            Console.ReadLine();
        }

        static int Add(int a, int b)
        {
            Console.WriteLine($"'Add()' invoked on thread {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(5000);
            return a + b;
        }

        static void AddComplete(IAsyncResult iar)
        {
            Console.WriteLine($"'AddComplete()' invoked on thread {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("Your addition is complete");

            AsyncResult ar = (AsyncResult)iar;
            BinaryOp b = (BinaryOp)ar.AsyncDelegate;
            Console.WriteLine($"Answer is: {b.EndInvoke(iar)}");
            string msg = (string)iar.AsyncState;
            Console.WriteLine(msg);
            isDone = true;
        }
    }
}
