using System;
using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SectorBalanceShared
{
    [Table("users")]
    public class User : BaseEntity
    {
        public User()
        {
            
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public Guid Id { get; set; }
       
       
        [Key]
        [Column("user_name")]
        [MaxLength(25)]
        public string UserName { get; set; }

        [Column("password")]
        [MaxLength(15)]
        public string Password { get; set; }

        [Column("active")]
        public bool Active { get; set; }
    }
}
