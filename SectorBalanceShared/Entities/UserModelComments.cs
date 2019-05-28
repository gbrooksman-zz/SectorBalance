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
        [Column("id")]
        public Guid Id { get; set; }

        [Column("model_id")]
        public Guid ModelId { get; set; }

        [Column("user_id")]
        public Guid UserlId { get; set; }

        [Column("comment")]
        [MaxLength(2000)]
        public string Comment { get; set; }

    }
}
