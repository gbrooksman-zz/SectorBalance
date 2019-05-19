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
    public class BaseManager
    {
        internal readonly IMemoryCache cache;
        internal readonly string connString;
        internal readonly IConfiguration config;

        public BaseManager(IMemoryCache _cache, IConfiguration _config)
        {
            cache = _cache;
            config = _config;

            connString = config.GetConnectionString("Default");
        }
    }
}
