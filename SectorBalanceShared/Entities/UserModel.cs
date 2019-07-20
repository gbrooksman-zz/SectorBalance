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
        public Guid Id { get; set; }

        public Guid UserId { get; set; }       

        [MaxLength(200)]
        public string Name { get; set; }

        public bool Active {get; set;}

        public DateTime StartDate {get; set;}

        public DateTime StopDate { get; set; }

        public int StartValue { get; set; } 

        public int StopValue { get; set; }

        public int Version { get; set; }

        public bool IsPrivate { get; set; }

        public Guid ModelVersionId { get; set; }
    }
}
