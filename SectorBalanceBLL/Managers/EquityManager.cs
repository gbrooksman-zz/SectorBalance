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
    public class EquityManager : BaseManager
    {
       
        // an equity is an investment vehilce identified by a equity that is included in the api's data

        public EquityManager(IMemoryCache _cache, IConfiguration _config) : base(_cache, _config)
        {

        }

        public async Task<ManagerResult<int>> GetEquitiesInModelsCount(Guid equityId)
        {
            ManagerResult<int> mgrResult = new ManagerResult<int>();
            
            try
            {
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    mgrResult.Entity = await db.QueryFirstOrDefaultAsync<int>(@"SELECT count(*)  
                                                                                FROM model_equities 
                                                                                WHERE equity_id = @p1 ", 
                                                                                new { p1 = equityId });
                }
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
                Log.Error("EquityManager::GetEquitiesInModelsCount",ex);
            } 

            return mgrResult;
        }


        public async Task<ManagerResult<List<Equity>>> GetList()
        {
            ManagerResult<List<Equity>> mgrResult = new ManagerResult<List<Equity>>();
            List<Equity> equityList = new List<Equity>();

            try
            {
                equityList = await cache.GetOrCreateAsync<List<Equity>>(CacheKeys.EQUITY_LIST, entry =>
                {
                    using (NpgsqlConnection db = new NpgsqlConnection(connString))
                    {
                        return Task.FromResult(db.Query<Equity>("SELECT * FROM equities").ToList());
                    }
                });

                mgrResult.Entity = equityList;
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
                mgrResult.Message = "EquityManager::GetList";
                Log.Error("EquityManager::GetList",ex);
            }
            
            return mgrResult;
        }

        public async Task<ManagerResult<Equity>> Get(Guid equityId)
        {
            ManagerResult<Equity> mgrResult = new ManagerResult<Equity>();
             
            try
            {
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    mgrResult.Entity = await db.QueryFirstOrDefaultAsync<Equity>(@"SELECT * 
                                                                                    FROM equities 
                                                                                    WHERE id = @p1 ", new { p1 = equityId });
                }
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
                mgrResult.Message = "EquityManager::Get";
                Log.Error("EquityManager::Get",ex);
            }
            
            return mgrResult;
        }

        public async Task<Equity> GetBySymbol(string symbol)
        {
            Equity equity = new Equity();
           
            equity = await cache.GetOrCreateAsync<Equity>(CacheKeys.EQUITY + symbol, entry =>
            {
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    return Task.FromResult(db.QueryFirstOrDefault<Equity>(@"SELECT * 
                                            FROM equities 
                                            WHERE LOWER(symbol) = @p1 ",
                                            new { p1 = symbol.ToLower() }));
                }
            });

            return equity;
        }
               
        #region CRUD

        public async Task<ManagerResult<Equity>> Save(Equity equity)
        {
            ManagerResult<Equity> mgrResult = new ManagerResult<Equity>();
            
            try
            {
                if (equity.Id == Guid.Empty)
                {
                    using (NpgsqlConnection db = new NpgsqlConnection(connString))
                    {
                        await db.InsertAsync(equity);
                    }
                }
                else
                {
                    using (NpgsqlConnection db = new NpgsqlConnection(connString))
                    {
                        await db.UpdateAsync(equity);
                    }
                }           
                mgrResult.Entity = equity;
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
                mgrResult.Message = "EquityManager::Save";
                Log.Error("EquityManager::Save",ex);
            } 

            return mgrResult;
        }

      

        public async Task<ManagerResult<bool>> Delete(Guid equityId)
        {
            ManagerResult<bool> mgrResult = new ManagerResult<bool>();
            Equity equity = new Equity()
            {
                Id = equityId
            };            
            
            try
            {   
                int count = GetEquitiesInModelsCount(equity.Id).Result.Entity;

                if (count == 0)
                {
                    using (NpgsqlConnection db = new NpgsqlConnection(connString))
                    {
                        mgrResult.Entity = await db.DeleteAsync(equity);
                    }
                }
                else
                {
                    mgrResult.Exception = new APIException( APIException.EQUITY_USED, 
                                                            APIException.EQUITY_USED_MESSAGE);
                }
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
                mgrResult.Message = "EquityManager::Save";
                Log.Error("EquityManager::Delete",ex);
            } 

            return mgrResult;
        }

        #endregion

    }

}