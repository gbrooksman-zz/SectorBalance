using System;
using NpgsqlTypes;

namespace SectorBalanceShared
{
    public class ModelSymbol
    {
        public ModelSymbol()
        {
            
        }

        [PgName("model_symbol_id")]
        public Guid Id { get; set; }

         [PgName("model_id")]
        public Guid ModelId { get; set; }

        [PgName("symbol")]
        public string Symbol { get; set; }      

        [PgName("percent")]
        public int Percent { get; set; }

    }
}
