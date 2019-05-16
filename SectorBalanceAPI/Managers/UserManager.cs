using System;
using System.Collections.Generic;
using Dapper;
using Npgsql;
using SectorBalanceShared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace SectorBalanceShared
{
    public class UserManager
    {
        private readonly IMemoryCache cache;
        private readonly string connString;
        private readonly IConfiguration config;

        public UserManager(IMemoryCache _cache, IConfiguration _config)
        {
            cache = _cache;
            config = _config;

            connString = config.GetConnectionString("Default");
        }

        public List<User> GetAllUsers()
        {
            using (NpgsqlConnection db = new NpgsqlConnection(connString))
            {
                return db.Query<User>("SELECT * FROM users WHERE active = True").ToList();
            }
        }

        public User GetOneById(Guid id)
        {
            using (NpgsqlConnection db = new NpgsqlConnection(connString))
            {
                return db.Query<User>("SELECT * FROM users WHERE id = @id",id).FirstOrDefault();
            }
        }

        public User GetOneByName(string userName)
        {
            using (NpgsqlConnection db = new NpgsqlConnection(connString))
            {
                return db.Query<User>("SELECT * FROM users WHERE user_name = @un",userName).FirstOrDefault();
            }
        }


        public User Validate(string userName, string password)
        {
            using (NpgsqlConnection db = new NpgsqlConnection(connString))
            {
                return db.Query<User>(@"SELECT * 
                                        FROM users 
                                        WHERE user_name = @un 
                                        AND password = @pw 
                                        AND active = true",new {userName, password}).FirstOrDefault();
            }
        }

    }
}
