using System;
using Newtonsoft.Json;
using System.Threading;

namespace binanceWssTest
{
    class TimeSync
    {
        public static long timeDelta { get; set; }
        public static long GetHostTimeMs(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return (long)Math.Floor(diff.TotalMilliseconds);
        }
        public static long GetServerTimeMs()
        {
            ServerTime sTime = new ServerTime();
            try
            {
                using (var WebClient = new System.Net.WebClient())
                {
                    var json = WebClient.DownloadString("https://api.binance.com/api/v1/time");
                    sTime = JsonConvert.DeserializeObject<ServerTime>(json);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Failed to get server time: {e.Message}");
                return 0;
            }
            return sTime.serverTime;
        }
        public static long GetTimeDelta()
        {
            DateTime now = DateTime.Now;
            long hostTimeMs = GetHostTimeMs(now);
            long serverTimeMs = GetServerTimeMs();
            if (serverTimeMs == 0)
                return 0;
            else if (serverTimeMs > hostTimeMs)
                return (serverTimeMs - hostTimeMs) * -1;
            else
                return hostTimeMs - serverTimeMs;
        }
        public static void UpdateTimeDelta()
        {
            while(true)
            {
                timeDelta = GetTimeDelta();
                Thread.Sleep(50000);
            }
        }
    }
}
