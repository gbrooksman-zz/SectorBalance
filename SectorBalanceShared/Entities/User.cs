using System;

namespace SectorBalanceShared
{
    public class User : BaseEntity
    {
        public User()
        {
            
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        public Guid Id { get; set; }

        public bool Active { get; set; }

    }
}
