using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SectorBalanceShared
{
    [Table("symbol_group_items")]
    public class SymbolGroupItem : BaseEntity
    {
        public SymbolGroupItem()
        {
            
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("group_id")]
        public Guid GroupId { get; set; }

        [Column("symbol_id")]
        public Guid SymbolId { get; set; }
    }
}