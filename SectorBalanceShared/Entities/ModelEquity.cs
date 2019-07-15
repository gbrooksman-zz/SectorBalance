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
        public Guid Id { get; set; }

        public Guid ModelId { get; set; }

        public Guid EquityID { get; set; }      

        public decimal Percent { get; set; }

        public decimal Cost { get; set; }

        public decimal Shares { get; set; }

        public int Version { get; set; }

        [NotMapped]
        public Equity Equity { get; set; }

        [NotMapped]
        public decimal LastPrice { get; set; }

        [NotMapped]
        public DateTime LastPriceDate { get; set; }

        [NotMapped]
        public decimal CurrentValue { get; set; }


    }
}
