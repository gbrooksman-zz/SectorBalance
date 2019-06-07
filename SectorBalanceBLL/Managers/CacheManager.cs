using System;
using System.Collections.Generic;
using Dapper;
using Npgsql;
using SectorBalanceShared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Dapper.FastCrud;
using Serilog;

namespace SectorBalanceBLL
{
    public class CacheManager : BaseManager
    {
        private EquityManager equityMgr;
        private EquityGroupManager equityGroupMgr;

        public CacheManager(IMemoryCache _cache, IConfiguration _config) : base(_cache, _config)
        {
             equityGroupMgr = new EquityGroupManager(_cache, _config);
             equityMgr = new EquityManager(_cache, _config);
        }

        public void RemoveAllKeys()
        {
            cache.Remove(CacheKeys.EQUITY_LIST);
            cache.Remove(CacheKeys.EQUITY_GROUP_LIST);

            var equityGroupList = equityGroupMgr.GetList().Result.Entity;        
            equityGroupList.ForEach(e =>  cache.Remove(e.Id) );
        }
    }
}