using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SectorBalanceShared
{
    [Table("equity_group_items")]
    public class EquityGroupItem : BaseEntity
    {
        public EquityGroupItem()
        {
            
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("group_id")]
        public Guid GroupId { get; set; }

        [Column("equity_id")]
        public Guid SymbolId { get; set; }
    }
}