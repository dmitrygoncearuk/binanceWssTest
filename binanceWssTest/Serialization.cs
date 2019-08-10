using System;

namespace binanceWssTest
{
    [Serializable]
    public struct EventStruct
    {
        public string e { get; set; } //eventType
        public long E { get; set; } //eventTime
        public string s { get; set; } //symbol
        public int a { get; set; } //eventId
        public string p { get; set; } //price
        public string q { get; set; } //quantity
        public int f { get; set; } //firstId
        public int l { get; set; } //secondId
        public long T { get; set; } //tradeTime
        public bool m { get; set; } //marketMaker
        public bool M { get; set; } //ignore
    }
    public class ServerTime
    {
        public long serverTime { get; set; }
    }
}
