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

        public int Percent { get; set; }

    }
}
