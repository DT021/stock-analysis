using System;
using core.Adapters.Stocks;

namespace core.Portfolio
{
    public struct ReviewEntry
    {
        public string Ticker { get; internal set; }
        public string Description { get; internal set; }
        public DateTimeOffset? Expiration { get; internal set; }
        public TimeSpan? TTL => Expiration == null ? (TimeSpan?)null : DateTimeOffset.UtcNow.Subtract(this.Expiration.Value);
        public bool IsNote { get; internal set; }
        public DateTimeOffset Created { get; internal set; }
        public StockAdvancedStats Stats { get; internal set; }
        public TickerPrice Price { get; internal set; }
    }
}