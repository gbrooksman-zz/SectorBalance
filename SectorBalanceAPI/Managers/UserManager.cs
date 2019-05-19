using System;
using System.Collections.Generic;
using Dapper;
using Npgsql;
using SectorBalanceShared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Dapper.FastCrud;

namespace SectorBalanceShared
{
    public class UserManager :BaseManager
    {
        private readonly IMemoryCache cache;
        private readonly string connString;
        private readonly IConfiguration config;

        public UserManager(IMemoryCache _cache, IConfiguration _config) : base(_cache, _config)
        {
            cache = _cache;
            config = _config;

            connString = config.GetConnectionString("Default");
        }

        public ManagerResult<List<User>> GetAllUsers()
        {
            ManagerResult<List<User>> mgrResult = new ManagerResult<List<User>>();
            List<User> users = new List<User>();
            
            try
            {
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                     users = db.Query<User>("SELECT * FROM users WHERE active = True").ToList();
                }
                mgrResult.ResultEntity = users;
            }
            catch(Exception ex)
            {
                mgrResult.ResultEntity = default(List<User>);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            } 

            return mgrResult;
        }

        public User GetOneById(Guid id)
        {
            ManagerResult<User> mgrResult = new ManagerResult<User>();
            User user = new User();
            
            try
            {
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    user =  db.Query<User>("SELECT * FROM users WHERE id = @id",id).FirstOrDefault();
                }
                mgrResult.ResultEntity = user;
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            }
            
            return user;
        }

        public User GetOneByName(string userName)
        {
            ManagerResult<User> mgrResult = new ManagerResult<User>();
            User user = new User();
              
            try
            {
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                   user = db.Query<User>("SELECT * FROM users WHERE user_name = @un",userName).FirstOrDefault();
                }
                mgrResult.ResultEntity = user;
            }
            catch(Exception ex)
            {
                mgrResult.ResultEntity = user;
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            }
            
            return user;
        }


        public bool Validate(string userName, string password)
        {
            ManagerResult<bool> mgrResult = new ManagerResult<bool>();
            User user = new User();
            bool ok = false;
              
            try
            {
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    user =  db.Query<User>(@"SELECT * 
                                            FROM users 
                                            WHERE user_name = @un 
                                            AND password = @pw 
                                            AND active = true",new {userName, password}).FirstOrDefault();
                
                    mgrResult.ResultEntity = (user != default(User));
                }
            }
            catch(Exception ex)
            {
                mgrResult.ResultEntity = ok;
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            }
            
            return ok;
        }

        public User Save(User user)
        {
            ManagerResult<User> mgrResult = new ManagerResult<User>();              
            try
            {
                if (user.Id == Guid.Empty)
                {
                    using (NpgsqlConnection db = new NpgsqlConnection(connString))
                    {
                        db.Insert(user);
                    }
                }
                else
                {
                    using (NpgsqlConnection db = new NpgsqlConnection(connString))
                    {
                        db.Update(user);
                    }
                }           
            }
            catch(Exception ex)
            {
                mgrResult.ResultEntity = user;
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            }
            
            return user;
        }

        public bool ToggleActive(User user)
        {
            bool ok = false;
            ManagerResult<bool> mgrResult = new ManagerResult<bool>();   

            try
            {                
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    user.Active = !user.Active;
                    ok = db.Update(user);
                }
            }
            catch(Exception ex)
            {
                mgrResult.ResultEntity = false;
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            }            
           
            return ok;
        }
    }
}
