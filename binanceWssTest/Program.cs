using System;
using System.Threading;
using Newtonsoft.Json;

namespace binanceWssTest
{
    class Program
    {
        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalMilliseconds);
        }

        static void Main(string[] args)  //wss://stream.binance.com:9443/ws/ethbtc@aggTrade
        {
            ServerTime serverTime;
            DateTime now = DateTime.Now; 
            using (var WebClient = new System.Net.WebClient())
            {
                var json = WebClient.DownloadString("https://api.binance.com/api/v1/time");
                serverTime = JsonConvert.DeserializeObject<ServerTime>(json);
            }
            Console.WriteLine(serverTime.serverTime);
            Console.WriteLine(ConvertToUnixTimestamp(now));
            Console.ReadLine();

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
