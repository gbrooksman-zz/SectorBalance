using System;
using NpgsqlTypes;

namespace SectorBalanceShared
{
    public class SymbolGroupItem : BaseEntity
    {
        public SymbolGroupItem()
        {
            
        }

        [PgName("id")]
        public Guid Id { get; set; }

        [PgName("group_id")]
        public Guid GroupId { get; set; }

        [PgName("symbol_id")]
        public Guid SymbolId { get; set; }
    }
}