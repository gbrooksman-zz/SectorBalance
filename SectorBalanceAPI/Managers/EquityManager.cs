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
    public class EquityManager : BaseManager
    {
       
        // an equity is an investment vehilce identified by a equity that is included in the api's data

        public EquityManager(IMemoryCache _cache, IConfiguration _config) : base(_cache, _config)
        {

        }

        public ManagerResult<int> GetEquitiesInModelsCount(Guid equityId)
        {
            ManagerResult<int> mgrResult = new ManagerResult<int>();
            
            try
            {  
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    mgrResult.Entity = db.Query<int>("SELECT count(*) FROM model_equities WHERE equity_id = @e",equityId).FirstOrDefault();                         
                }
            }
            catch(Exception ex)
            {
                mgrResult.Entity = default(int);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
                Log.Error("EquityManager::GetEquitiesInModelsCount",ex);
            } 

            return mgrResult;
        }


        public ManagerResult<List<Equity>> GetList()
        {
            ManagerResult<List<Equity>> mgrResult = new ManagerResult<List<Equity>>();
            List<Equity> equityList = new List<Equity>();
              
            try
            {
                equityList = cache.GetOrCreate<List<Equity>>(CacheKeys.EQUITY_LIST, entry =>
                {
                    using (NpgsqlConnection db = new NpgsqlConnection(connString))
                    {                    
                        return db.Query<Equity>("SELECT * FROM equities").ToList();
                    }
                });

                mgrResult.Entity = equityList;
            }
            catch(Exception ex)
            {
                mgrResult.Entity = default(List<Equity>);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
                Log.Error("EquityManager::GetList",ex);
            }
            
            return mgrResult;
        }

        public ManagerResult<Equity> Get(Guid equityId)
        {
            ManagerResult<Equity> mgrResult = new ManagerResult<Equity>();
             
            try
            {
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                   mgrResult.Entity = db.Query<Equity>(@"SELECT * 
                                                FROM equities 
                                                WHERE id = @eid", equityId).FirstOrDefault();  
                }
            }
            catch(Exception ex)
            {
                mgrResult.Entity = default(Equity);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
                Log.Error("EquityManager::Get",ex);
            }
            
            return mgrResult;
        }

        public ManagerResult<Equity> Save(Equity equity)
        {
            ManagerResult<Equity> mgrResult = new ManagerResult<Equity>();
            
            try
            {
                if (equity.Id == Guid.Empty)
                {
                    using (NpgsqlConnection db = new NpgsqlConnection(connString))
                    {
                        db.Insert(equity);
                    }
                }
                else
                {
                    using (NpgsqlConnection db = new NpgsqlConnection(connString))
                    {
                        db.Update(equity);
                    }
                }           
                mgrResult.Entity = equity;
            }
            catch(Exception ex)
            {
                mgrResult.Entity = default(Equity);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
                Log.Error("EquityManager::Save",ex);
            } 

            return mgrResult;
        }

      

        public ManagerResult<bool> Delete(Guid equityId)
        {
            ManagerResult<bool> mgrResult = new ManagerResult<bool>();
            bool ok = false;

            Equity equity = new Equity()
            {
                Id = equityId
            };
            
            
            try
            {   
                int count = this.GetEquitiesInModelsCount(equity.Id).Entity;

                if (count == 0)
                {
                    using (NpgsqlConnection db = new NpgsqlConnection(connString))
                    {
                        ok = db.Delete(equity);                         
                    }
                    mgrResult.Entity = ok;
                }
                else
                {
                    mgrResult.Entity = false;
                    mgrResult.Success = false;
                    mgrResult.Exception = new APIException( APIException.EQUITY_USED, 
                                                            APIException.EQUITY_USED_MESSAGE);
                }
            }
            catch(Exception ex)
            {
                mgrResult.Entity = false;
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
                Log.Error("EquityManager::Delete",ex);
            } 

            return mgrResult;
        }
    }

}