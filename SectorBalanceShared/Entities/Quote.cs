using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SectorBalanceShared
{
    [Table("quotes")]
    public class Quote : BaseEntity
    {
        public Quote()
        {
            
        }

        [Key]
        [Column("date")]
        public DateTime Date { get; set; }

        [Key]
        [Column("equity_id")]
        public Guid EquityId { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("volume")]
        public int Volume { get; set; }
        
        [Column("rate_of_change")]
        public decimal RateOfChange { get; set; }
    }
}
