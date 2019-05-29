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
    //a quote is the price of an equity on a given day
    public class QuoteManager : BaseManager
    {
        EquityManager eqMgr;
        EquityGroupManager eqGroupMgr;

        public QuoteManager(IMemoryCache _cache, IConfiguration _config) : base(_cache, _config)
        {
            eqMgr = new EquityManager(_cache, _config);
            eqGroupMgr = new EquityGroupManager(_cache, _config);
        }

        public ManagerResult<List<Quote>> GetEquityGroupQuoteList(EquityGroup equityGroup, DateTime startdate, DateTime stopdate)
        {
            ManagerResult<List<Quote>> mgrResult = new ManagerResult<List<Quote>>();
            
            using (NpgsqlConnection db = new NpgsqlConnection(base.connString))
            {
               var equityList = eqGroupMgr.GetGroupItemsList(equityGroup).Entity; 

                foreach (EquityGroupItem equity in equityList)
                {
                    mgrResult.Entity.AddRange(
                        GetByEquityIdAndDateRange(equity.Id, startdate, stopdate).Entity);
                }
            }

            return mgrResult;
        }

        public ManagerResult<List<Quote>> GetByEquityIdAndDateRange(Guid equityId, DateTime startdate, DateTime stopdate)
        {
            ManagerResult<List<Quote>> mgrResult = new ManagerResult<List<Quote>>();

            List<Quote> quoteList = GetByEquityId(equityId);
            
            mgrResult.Entity =  quoteList.Where(q => q.Date >= startdate && q.Date <= stopdate).ToList();            

            return mgrResult;
        }

        private List<Quote> GetByEquityId(Guid equityId)
        {
            return cache.GetOrCreate<List<Quote>>(CacheKeys.QUOTE_LIST + equityId, entry =>
                        {
                            using NpgsqlConnection db = new NpgsqlConnection(connString);
                            return db.Query<Quote>(@"SELECT * FROM quotes 
                                                        WHERE equity_id = @p1", new { p1 = equityId } ).ToList();
                        });
        }


        public ManagerResult<Quote> GetByEquityIdAndDate(Guid equityId, DateTime date)
        {
            ManagerResult<Quote> mgrResult = new ManagerResult<Quote>();

            List<Quote> quoteList = GetByEquityId(equityId);
            
            mgrResult.Entity =  quoteList.Where(q => q.Date == date).FirstOrDefault();  

            return mgrResult;
        }

       public ManagerResult<Quote> Add(Quote quote)
        {    
            ManagerResult<Quote> mgrResult = new ManagerResult<Quote>();

            try
            {
                if (eqMgr.Get(quote.EquityId).Entity == default(Equity)) 
                {
                    using NpgsqlConnection db = new NpgsqlConnection(base.connString);
                    db.Insert(quote);
                }
                else
                {
                    quote = new Quote();
                }
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
                Log.Error("QuoteManager::Add",ex);
            }
             
            return mgrResult;
        }


       public ManagerResult<bool> Delete(Quote quote)
       {     
            ManagerResult<bool> mgrResult = new ManagerResult<bool>();

            if (eqMgr.Get(quote.EquityId).Entity != default(Equity)) 
            {
                using NpgsqlConnection db = new NpgsqlConnection(base.connString);
                db.Delete(quote);
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
