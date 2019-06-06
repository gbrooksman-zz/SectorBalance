using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SectorBalanceShared
{
    public class BaseEntity
    {
        public BaseEntity ()
        {
            
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
