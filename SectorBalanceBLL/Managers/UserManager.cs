using System;
using System.Collections.Generic;
using Dapper;
using Npgsql;
using SectorBalanceShared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Dapper.FastCrud;
using System.Threading.Tasks;

namespace SectorBalanceBLL
{
    public class UserManager : BaseManager
    {
        public UserManager(IMemoryCache _cache, IConfiguration _config) : base(_cache, _config)
        {

        }

        public async Task<ManagerResult<List<User>>> GetAllUsers()
        {
            ManagerResult<List<User>> mgrResult = new ManagerResult<List<User>>();
            
            try
            {
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    mgrResult.Entity = db.QueryAsync<User>("SELECT * FROM users WHERE active = True").Result.ToList();
                }
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            } 

            return mgrResult;
        }

        public async Task<ManagerResult<User>> GetOneById(Guid id)
        {
            ManagerResult<User> mgrResult = new ManagerResult<User>();
            
            try
            {
                using NpgsqlConnection db = new NpgsqlConnection(connString);
                mgrResult.Entity = await db.QueryFirstOrDefaultAsync<User>("SELECT * FROM users WHERE id = @p1", new { p1 = id });
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            }
            
            return mgrResult;
        }

        public async Task<ManagerResult<User>> GetOneByName(string userName)
        {
            ManagerResult<User> mgrResult = new ManagerResult<User>();
            User user = new User();
              
            try
            {
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                   user = await db.QueryFirstOrDefaultAsync<User>(@"SELECT * 
                                                FROM users 
                                                WHERE user_name = @p1", 
                                                new { p1 = userName } );
                }
                mgrResult.Entity = user;
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            }
            
            return mgrResult;
        }


        public async Task<ManagerResult<bool>> Validate(string userName, string password)
        {
            ManagerResult<bool> mgrResult = new ManagerResult<bool>();

            try
            {
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    User user = await db.QueryFirstOrDefaultAsync<User>(@"SELECT * 
                                            FROM users 
                                            WHERE user_name = @p1 
                                            AND password = @p2 
                                            AND active = true", 
                                            new { p1 = userName, p2 = password });

                    mgrResult.Entity = (user != default(User));
                }
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            }
            
            return mgrResult;
        }

        public async Task<ManagerResult<User>> Save(User user)
        {
            ManagerResult<User> mgrResult = new ManagerResult<User>();              
            try
            {
                if (user.Id == Guid.Empty)
                {
                    using NpgsqlConnection db = new NpgsqlConnection(connString);
                    await db.InsertAsync(user);
                }
                else
                {
                    using NpgsqlConnection db = new NpgsqlConnection(connString);
                    await db.UpdateAsync(user);
                }           
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            }
            
            return mgrResult;
        }
    }
}
