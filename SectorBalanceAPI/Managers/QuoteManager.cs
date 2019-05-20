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

namespace SectorBalanceAPI
{
    public class QuoteManager : BaseManager
    {

        EquityManager eqMgr;

        public QuoteManager(IMemoryCache _cache, IConfiguration _config) : base(_cache, _config)
        {
            eqMgr = new EquityManager(_cache, _config);
        }

        public ManagerResult<Quote> GetBySymbolAndDate(string symbol, DateTime date)
        {
            ManagerResult<Quote> mgrResult = new ManagerResult<Quote>();

            using (NpgsqlConnection db = new NpgsqlConnection(base.connString))
            {
               mgrResult.Entity =  db.Find<Quote>().Where( q => q.Symbol == symbol 
                                                        && q.Date == date).FirstOrDefault();
            }

            return mgrResult;
        }

       public ManagerResult<Quote> Add(Quote quote)
        {    
            ManagerResult<Quote> mgrResult = new ManagerResult<Quote>();

            try
            {
                if (eqMgr.Get(quote.Symbol).Entity == default(Equity)) 
                {
                    using (NpgsqlConnection db = new NpgsqlConnection(base.connString))
                    {
                        db.Insert(quote);
                    }
                }
                else
                {
                    quote = new Quote();
                }
            }
            catch(Exception ex)
            {
                mgrResult.Entity = default(Quote);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
                Log.Error("QuoteManager::Add",ex);
            }
             
            return mgrResult;
        }


       public ManagerResult<bool> Delete(Quote quote)
       {     
            ManagerResult<bool> mgrResult = new ManagerResult<bool>();

            if (eqMgr.Get(quote.Symbol).Entity != default(Equity)) 
            {
                using (NpgsqlConnection db = new NpgsqlConnection(base.connString))
                {
                    db.Delete(quote);
                }
            }
            else
            {
                mgrResult.Entity = false;
                mgrResult.Success = false;
            }
             
            return mgrResult;
        }
    }
}
