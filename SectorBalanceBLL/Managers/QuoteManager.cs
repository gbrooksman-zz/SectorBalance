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
using System.Threading.Tasks;

namespace SectorBalanceBLL
{
    //a quote is the price of an equity on a given day
    public class QuoteManager : BaseManager
    {
        readonly EquityManager eqMgr;
        readonly EquityGroupManager eqGroupMgr;

        public QuoteManager(IMemoryCache _cache, IConfiguration _config) : base(_cache, _config)
        {
            eqMgr = new EquityManager(_cache, _config);
            eqGroupMgr = new EquityGroupManager(_cache, _config);
        }

        public async Task<ManagerResult<List<Quote>>> GetEquityGroupQuoteList(EquityGroup equityGroup, DateTime startdate, DateTime stopdate)
        {
            ManagerResult<List<Quote>> mgrResult = new ManagerResult<List<Quote>>();
            
            using (NpgsqlConnection db = new NpgsqlConnection(base.connString))
            {
                var mgrGroupItemsResult = await eqGroupMgr.GetGroupItemsList(equityGroup.Id);

                foreach (EquityGroupItem equity in mgrGroupItemsResult.Entity)
                {
                    mgrResult.Entity.AddRange(
                        GetByEquityIdAndDateRange(equity.Id, startdate, stopdate).Result.Entity);
                }
            }

            return mgrResult;
        }

        public async Task<ManagerResult<List<Quote>>> GetByEquityIdAndDateRange(Guid equityId, DateTime startdate, DateTime stopdate)
        {
            ManagerResult<List<Quote>> mgrResult = new ManagerResult<List<Quote>>();

            List<Quote> quoteList = await GetByEquityId(equityId);
            
            mgrResult.Entity =  quoteList.Where(q => q.Date >= startdate && q.Date <= stopdate).ToList();            

            return mgrResult;
        }

        private async Task<List<Quote>> GetByEquityId(Guid equityId)
        {
            return await cache.GetOrCreateAsync<List<Quote>>(CacheKeys.QUOTE_LIST + equityId, entry =>
                        {
                            using (NpgsqlConnection db = new NpgsqlConnection(connString))
                            {
                                return Task.FromResult(db.Query<Quote>(@"   SELECT * 
                                                                            FROM quotes 
                                                                            WHERE equity_id = @p1",
                                                                            new { p1 = equityId }).ToList());
                            }
                        });
        }


        public async Task<ManagerResult<Quote>> GetByEquityIdAndDate(Guid equityId, DateTime date)
        {
            ManagerResult<Quote> mgrResult = new ManagerResult<Quote>();

            using (NpgsqlConnection db = new NpgsqlConnection(connString))
            {
                mgrResult.Entity = await db.QueryFirstOrDefaultAsync<Quote>(@"  SELECT * 
                                                                                FROM quotes 
                                                                                WHERE equity_id = @p1 
                                                                                AND date = @p2",
                                                                                new { p1 = equityId, p2 = date });
            }

            return mgrResult;
        }

       public async Task<ManagerResult<Quote>> Add(Quote quote)
        {    
            ManagerResult<Quote> mgrResult = new ManagerResult<Quote>();

            try
            {
                if (eqMgr.Get(quote.EquityId).Result.Entity == default(Equity)) 
                {
                    using (NpgsqlConnection db = new NpgsqlConnection(base.connString))
                    {
                        await db.InsertAsync(quote);
                    }
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


       public async Task<ManagerResult<bool>> Delete(Quote quote)
       {     
            ManagerResult<bool> mgrResult = new ManagerResult<bool>();

            if (eqMgr.Get(quote.EquityId).Result.Entity != default(Equity)) 
            {
                using (NpgsqlConnection db = new NpgsqlConnection(base.connString))
                {
                    await db.DeleteAsync(quote);
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
