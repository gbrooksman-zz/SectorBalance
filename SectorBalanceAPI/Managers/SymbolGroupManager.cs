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
    public class SymbolGroupManager : BaseManager
    {
    //     private readonly IMemoryCache cache;
    //     private readonly string connString;
    //     private readonly IConfiguration config;

        public SymbolGroupManager(IMemoryCache _cache, IConfiguration _config) : base(_cache, _config)
        {
            // base.cache = _cache;
            // base.config = _config;

            // basde.connString = config.GetConnectionString("Default");
        }
    }
}
