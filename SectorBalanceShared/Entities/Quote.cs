using System;
using NpgsqlTypes;

namespace SectorBalanceShared
{
    public class Quote : BaseEntity
    {

        public Quote()
        {
            
        }

        [PgName("symbol")]
        public string Symbol { get; set; }

         [PgName("price")]
        public decimal Price { get; set; }

         [PgName("date")]
        public DateTime Date { get; set; }

         [PgName("volume")]
        public int Volume { get; set; }

    }
}
