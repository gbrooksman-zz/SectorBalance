using System;
using Npgsql.PostgresTypes;
using Npgsql;
using NpgsqlTypes;

namespace SectorBalanceShared
{
    public class UserModel : BaseEntity
    {
        public UserModel()
        {
            
        }

         public Guid Id { get; set; }

        public Guid UserId { get; set; }       
        
        public string Name { get; set; }
        
        public bool IsActive {get; set;}

        public DateTime StartDate {get; set;}

        public DateTime StopDate { get; set; }

        public int StartValue { get; set; } 

        public int StopValue { get; set; }
    }
}
