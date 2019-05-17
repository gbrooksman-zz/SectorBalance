using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SectorBalanceShared
{
    [Table("model_symbols")]
    public class ModelSymbol
    {
        public ModelSymbol()
        {
            
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("model_symbol_id")]
        public Guid Id { get; set; }

        [Column("model_id")]
        public Guid ModelId { get; set; }

        [Column("symbol")]
        public string Symbol { get; set; }      

        [Column("percent")]
        public int Percent { get; set; }

    }
}
