using System;

namespace core.Options
{
    public partial class OptionDetail
    {
        public string Symbol { get; set; }

        public string Id { get; set; }

        public string ExpirationDate { get; set; }

        public long ContractSize { get; set; }

        public double StrikePrice { get; set; }

        public double ClosingPrice { get; set; }

        public string Side { get; set; }

        public long Volume { get; set; }

        public long OpenInterest { get; set; }

        public double Bid { get; set; }

        public double Ask { get; set; }

        public DateTimeOffset LastUpdated { get; set; }
        public string OptionType => this.Side;
        public bool IsCall => this.Side == "call";
        public bool IsPut => this.Side == "put";
        public double Spread => (this.Ask - this.Bid);
    }
}