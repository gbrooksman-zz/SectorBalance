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
        [Column("id")]
        public Guid Id {get; set;}

        [Column("name")]
        [MaxLength(200)]
        public string Name { get; set; }
        
        [Column("symbol")]
        [MaxLength(10)]
        public string Symbol { get; set; }
    }
}