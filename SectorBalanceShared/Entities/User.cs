using System;
using NpgsqlTypes;

namespace SectorBalanceShared
{
    public class User : BaseEntity
    {
        public User()
        {
            
        }

        [PgName("user_name")]
        public string UserName { get; set; }

        [PgName("password")]
        public string Password { get; set; }

        [PgName("id")]
        public Guid Id { get; set; }
 
        [PgName("active")]
        public bool Active { get; set; }

    }
}
