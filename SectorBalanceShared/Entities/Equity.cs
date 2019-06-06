using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SectorBalanceShared
{
    [Table("equities")]
    public class Equity : BaseEntity
    {
        public Equity()
        {
            
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id {get; set;}

        [MaxLength(200)]
        public string SymbolName { get; set; }
        
        [MaxLength(10)]
        public string Symbol { get; set; }
    }
}