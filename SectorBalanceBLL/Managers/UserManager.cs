using System;
using System.Collections.Generic;
using Dapper;
using Npgsql;
using SectorBalanceShared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Dapper.FastCrud;

namespace SectorBalanceBLL
{
    public class UserManager : BaseManager
    {
        public UserManager(IMemoryCache _cache, IConfiguration _config) : base(_cache, _config)
        {

        }

        public ManagerResult<List<User>> GetAllUsers()
        {
            ManagerResult<List<User>> mgrResult = new ManagerResult<List<User>>();
            
            try
            {
                using NpgsqlConnection db = new NpgsqlConnection(connString);
                mgrResult.Entity = db.Query<User>("SELECT * FROM users WHERE active = True").ToList();
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            } 

            return mgrResult;
        }

        public ManagerResult<User> GetOneById(Guid id)
        {
            ManagerResult<User> mgrResult = new ManagerResult<User>();
            
            try
            {
                using NpgsqlConnection db = new NpgsqlConnection(connString);
                mgrResult.Entity = db.Query<User>("SELECT * FROM users WHERE id = @p1", new { p1 = id }).FirstOrDefault();
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            }
            
            return mgrResult;
        }

        public ManagerResult<User> GetOneByName(string userName)
        {
            ManagerResult<User> mgrResult = new ManagerResult<User>();
            User user = new User();
              
            try
            {
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                   user = db.Query<User>("SELECT * FROM users WHERE user_name = @p1", new { p1 = userName } ).FirstOrDefault();
                }
                mgrResult.Entity = user;
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            }
            
            return mgrResult;
        }


        public ManagerResult<bool> Validate(string userName, string password)
        {
            ManagerResult<bool> mgrResult = new ManagerResult<bool>();

            try
            {
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    User user = db.Query<User>(@"SELECT * 
                                            FROM users 
                                            WHERE user_name = @p1 
                                            AND password = @p2 
                                            AND active = true", new { p1 = userName, p2 = password }).FirstOrDefault();

                    mgrResult.Entity = (user != default(User));
                }
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            }
            
            return mgrResult;
        }

        public ManagerResult<User> Save(User user)
        {
            ManagerResult<User> mgrResult = new ManagerResult<User>();              
            try
            {
                if (user.Id == Guid.Empty)
                {
                    using NpgsqlConnection db = new NpgsqlConnection(connString);
                    db.Insert(user);
                }
                else
                {
                    using NpgsqlConnection db = new NpgsqlConnection(connString);
                    db.Update(user);
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
