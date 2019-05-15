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

        [PgName("model_id")]
        public Guid Id { get; set; }

         [PgName("user_id")]
        public Guid UserId { get; set; }       
        
         [PgName("name")]
        public string Name { get; set; }
        
         [PgName("active")]
        public bool IsActive {get; set;}

         [PgName("start_date")]
        public DateTime StartDate {get; set;}

        [PgName("syop_date")]
        public DateTime StopDate { get; set; }

        [PgName("start_value")]
        public int StartValue { get; set; } 

         [PgName("stop_value")]
        public int StopValue { get; set; }
    }
}
