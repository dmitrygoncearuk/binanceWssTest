using System;
using WebSocketSharp;
using System.Threading;
using Newtonsoft.Json;

namespace binanceWssTest
{
    class Connection
    {
        public string ticker { get; set; }
        public CircularBuffer<EventStruct> buffer { get; set; }
        public bool connectionIsAlive { get; set; }
        public Connection()
        {
            ticker = "ethbtc";
            buffer = new CircularBuffer<EventStruct>(20);
        }
        public Connection(string _ticker)
        {
            ticker = _ticker.ToLower();
            buffer = new CircularBuffer<EventStruct>(20);
        }
        public Connection(string _ticker, int bufferSize)
        {
            ticker = _ticker.ToLower();
            buffer = new CircularBuffer<EventStruct>(bufferSize);
        }

        public void Write()
        {
            using (var ws = new WebSocket($"wss://stream.binance.com:9443/ws/{ticker}@aggTrade"))
            {
                ws.OnMessage += (sender, e) =>
                buffer.Input(JsonConvert.DeserializeObject<EventStruct>(e.Data));
                connectionIsAlive = ws.IsAlive;
                ws.Connect();
                Console.ReadKey(true);
            }
        }

        public void Read()
        {
            while (true)
            {
                if (!buffer.IsEmpty())
                {
                    EventStruct st = buffer.Output();
                    Console.WriteLine($"Ticker: {ticker} | Event Id: {st.a}; Event time: {st.E}; Price: {st.p}; Quantity: {st.q}");
                }
                else
                    Thread.Sleep(300);
            }
        }
    }
}
