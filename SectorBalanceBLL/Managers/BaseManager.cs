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

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
    }

    public static class CacheKeys
    {
        public static readonly string EQUITY_LIST = "equity_list";

        public static readonly string EQUITY_GROUP_LIST = "equity_group_list";

        public static readonly string QUOTE_LIST = "quote_list";

        public static readonly string LAST_QUOTE_DATE = "last_quote_date";

        public static readonly string EQUITY_ITEMS_LIST = "equity_ist";

        public static readonly string EQUITY = "equity";

        public static readonly string TRADING_DATES = "trading_dates";

        public static readonly string MODEL_LIST = "model_list";
    }
}
