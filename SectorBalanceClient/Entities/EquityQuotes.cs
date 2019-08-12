using SectorBalanceShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SectorBalanceClient.Entities
{
    public class EquityQuotes
    {
        public EquityQuotes()
        {

        }

        public Equity Equity { get; set; }

        public List<Quote> Quotes { get; set; }
    }
}
