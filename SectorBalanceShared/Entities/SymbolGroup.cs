using System;
using NpgsqlTypes;

namespace SectorBalanceShared
{
    public class SymbolGroup : BaseEntity
    {
        public SymbolGroup()
        {
            
        }

        [PgName("id")]
        public Guid Id { get; set; }

        [PgName("name")]
        public string Name { get; set; }
    }
}