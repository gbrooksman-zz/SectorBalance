using System;
using NpgsqlTypes;

namespace SectorBalanceShared
{
    public class Equity : BaseEntity
    {
        public Equity()
        {
            
        }

        [PgName("id")]
        public Guid Id {get; set;}

        [PgName("name")]
        public string Name { get; set; }
        
        [PgName("symbol")]
        public string Symbol { get; set; }
    }
}