using System;

namespace SectorBalanceShared
{
    public class Quote : BaseEntity
    {

        public Quote()
        {
            
        }

        public string Symbol { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public int Volume { get; set; }

    }
}
