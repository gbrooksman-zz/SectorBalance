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
        public Guid Id { get; set; }

        public Guid GroupId { get; set; }

        public Guid EquityId { get; set; }
    }
}