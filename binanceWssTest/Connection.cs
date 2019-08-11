using System;
using WebSocketSharp;
using System.Threading;
using Newtonsoft.Json;

namespace binanceWssTest
{
    class Connection
    {
        public WebSocket ws = new WebSocket("wss://stream.binance.com:9443/ws/");
        public string ticker { get; set; }
        public CircularBuffer<EventStruct> buffer { get; set; }
        public bool connectionIsAlive { get; set; }

        public Connection(string _ticker)
        {
            ticker = _ticker.ToLower();
            buffer = new CircularBuffer<EventStruct>(50);
        }
        public Connection(string _ticker, int bufferSize)
        {
            ticker = _ticker.ToLower();
            buffer = new CircularBuffer<EventStruct>(bufferSize);
        }

        public void Write()
        {
            try
            {
                using (ws = new WebSocket($"wss://stream.binance.com:9443/ws/{ticker}@aggTrade"))
                {
                    ws.OnMessage += (sender, e) =>
                    buffer.Input(JsonConvert.DeserializeObject<EventStruct>(e.Data));
                    connectionIsAlive = ws.IsAlive;
                    ws.Connect();
                    Console.ReadKey(true);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Failed to connect {ticker}: {e.Message}");
            }
        }

        public void Read()
        {            
            while (true)
            {
                if (!buffer.IsEmpty())
                {
                    EventStruct st = buffer.Output();
                    var eventTime = (new DateTime(1970, 1, 1)).AddMilliseconds(st.E + TimeSync.timeDelta);
                    Console.WriteLine($"Ticker: {ticker} | Event Id: {st.a}; Event time: {eventTime}; Price: {st.p}; Quantity: {st.q}");
                }
                else
                    Thread.Sleep(300);
            }
        }
    }
}
