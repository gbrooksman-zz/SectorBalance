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
        [Column("symbol")]
        public string Symbol { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("volume")]
        public int Volume { get; set; }
    }
}
