using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SectorBalanceShared.Entities
{
    public class UserModelComments : BaseEntity
    {
        public UserModelComments()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid ModelId { get; set; }

        public Guid UserId { get; set; }

        [MaxLength(2000)]
        public string Comment { get; set; }
    }
}
