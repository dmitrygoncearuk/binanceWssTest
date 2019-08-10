using System;
using System.Threading;

namespace binanceWssTest
{
    class Program
    {
        static void Main(string[] args)  //wss://stream.binance.com:9443/ws/ethbtc@aggTrade
        {
            Connection con1 = new Connection("ethbtc");
            Connection con2 = new Connection("ltcbtc");
            Thread t1 = new Thread(new ThreadStart(con1.Read));
            t1.Start();
            Thread t2 = new Thread(new ThreadStart(con1.Write));
            t2.Start();
            Thread t3 = new Thread(new ThreadStart(con2.Read));
            t3.Start();
            Thread t4 = new Thread(new ThreadStart(con2.Write));
            t4.Start();

            Console.ReadKey();
        }
    }
}
