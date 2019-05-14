using System;
using NpgsqlTypes;

namespace SectorBalanceShared
{
    public class BaseEntity
    {
        public BaseEntity ()
        {
            
        }

        [PgName("created_at")]
        public DateTime CreatedAt { get; set; }

        [PgName("updated_at")]
        public DateTime UpdatedAt { get; set; }


    }
}
