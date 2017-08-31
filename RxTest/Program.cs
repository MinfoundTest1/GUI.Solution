using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Timers;

namespace RxTest
{
    class Program
    {
        static T GetValue<T>(string strValue) where T : class
        {
            T a = null;
            return a;
        } 
       
        // I do this for the first branch.
        static void Main(string[] args)
        {
            try
            {
                //Observable.Interval(TimeSpan.FromSeconds(1))
                //    .Select(x => GetValue())
                //    .Subscribe(Console.WriteLine);
                var queue = new BufferBlock<string>();
     
                queue.AsObservable()
                    .Subscribe(s => Console.WriteLine($"Got message: {s}"));

                Observable.Interval(TimeSpan.FromSeconds(1))
                    .Subscribe(t => queue.Post(t.ToString()));

                var bq = new BlockingCollection<int>();
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

        }

        static IEnumerable<T> Unfold<T>(T seed, Func<T, T> accumulator)
        {
            var nextValue = seed;
            while (true)
            {
                yield return nextValue;
                nextValue = accumulator(nextValue);
            }
        }
    }
}
