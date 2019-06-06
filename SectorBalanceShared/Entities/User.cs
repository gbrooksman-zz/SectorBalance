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
        public Guid Id { get; set; }       
       
        [Key]
        [MaxLength(25)]
        public string UserName { get; set; }

        [MaxLength(15)]
        public string Password { get; set; }

        public bool Active { get; set; }
    }
}
