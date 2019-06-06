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
        public DateTime Date { get; set; }

        [Key]

        public Guid EquityId { get; set; }

        public decimal Price { get; set; }

        public int Volume { get; set; }

        public decimal RateOfChange { get; set; }
    }
}
