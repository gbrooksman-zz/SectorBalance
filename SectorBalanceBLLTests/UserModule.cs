using System;
using Xunit;
using SectorBalanceShared;
using Npgsql;
using SectorBalanceBLL;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace SectorBalanceBLLTests
{
    
    public class UserModule : BaseModule
    {
        //private UserManager userMgr;

        //private readonly IMemoryCache _memoryCache;
        //private readonly IConfiguration _config;

        [Fact]
        public void UserMethods()
        {         
            //userMgr = new UserManager(_memoryCache, _config);
            
            //ManagerResult<User> user = AddUser();


        }

        //private ManagerResult<User> AddUser()
        //{  
        //    //User user = new User
        //    //{
        //    //    UserName = "test_user",
        //    //    Password = "x"
        //    //};

        //    //return userMgr.Save(user);

        //}        
    }
}
