using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SectorBalanceShared
{
    [Table("user_models")]
    public class UserModel : BaseEntity
    {
        public UserModel()
        {
            
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }       
        
        [Column("name")]
        [MaxLength(200)]
        public string Name { get; set; }
        
        [Column("active")]
        public bool Active {get; set;}

        [Column("start_date")]
        public DateTime StartDate {get; set;}

        [Column("stop_date")]
        public DateTime StopDate { get; set; }

        [Column("start_value")]
        public int StartValue { get; set; } 

        [Column("stop_value")]
        public int StopValue { get; set; }

        [Column("is_private")]
        public bool IsPrivate { get; set; }

    }
}
