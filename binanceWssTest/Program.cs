using System;
using System.Threading;
using System.Collections.Generic;

namespace binanceWssTest
{
    class Program
    {
        static void Main(string[] args)
        {
            long timeDelta = TimeSync.timeDelta;
            Thread t0 = new Thread(new ThreadStart(TimeSync.UpdateTimeDelta));
            t0.Start();

            Console.Write("Input markets to monitor divided by coma. Ex(ethbtc,ltceth): ");
            string[] marketNames = Console.ReadLine().Split(',');
            List<Connection> markets = new List<Connection>();
            List<Thread> threads = new List<Thread>();

            foreach (string m in marketNames)
            {
                markets.Add(new Connection(m));
            }

            foreach (Connection m in markets)
            {
                threads.Add(new Thread(new ThreadStart(m.Write)));
                threads.Add(new Thread(new ThreadStart(m.Read)));
            }

            foreach (Thread t in threads)
            {
                if (threads.IndexOf(t) % 2 == 0)
                    t.Priority = ThreadPriority.Highest;
                else
                    t.Priority = ThreadPriority.Lowest;
                t.Start();
            }

            ConsoleKeyInfo k;
            Console.WriteLine("Press ESC to exit...");
            while (true)
            {
                k = Console.ReadKey(true);
                if (k.Key == ConsoleKey.Escape)
                {
                    foreach (Connection m in markets)
                    {
                        m.ws.Close();
                    }
                    Environment.Exit(1);
                }
            }
        }
    }
}
