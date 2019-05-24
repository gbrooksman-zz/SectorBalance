using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SectorBalanceShared
{
    [Table("model_equities")]
    public class ModelEquity : BaseEntity
    {
        public ModelEquity()
        {
            
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("model_id")]
        public Guid ModelId { get; set; }

        [Column("equity_id")]
        public Guid EquityID { get; set; }      

        [Column("percent")]
        public int Percent { get; set; }

    }
}
