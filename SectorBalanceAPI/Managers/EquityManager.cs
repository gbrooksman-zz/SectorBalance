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
    public class EquityManager : BaseManager
    {

        public EquityManager(IMemoryCache _cache, IConfiguration _config) : base(_cache, _config)
        {

        }


         public ManagerResult<Equity> Get(string symbol)
        {
            ManagerResult<Equity> mgrResult = new ManagerResult<Equity>();
            Equity equity = new Equity();
              
            try
            {
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                   equity = db.Query<Equity>("SELECT * FROM users WHERE symbol = @s",symbol).FirstOrDefault();
                }
                mgrResult.ResultEntity = equity;
            }
            catch(Exception ex)
            {
                mgrResult.ResultEntity = default(Equity);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            }
            
            return mgrResult;
        }
    }

}