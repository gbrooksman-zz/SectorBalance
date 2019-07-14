using System;
using System.Collections.Generic;
using Dapper;
using Npgsql;
using SectorBalanceShared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Dapper.FastCrud;
using System.Threading.Tasks;

namespace SectorBalanceBLL
{
    public class UserModelManager : BaseManager
    {
        private EquityManager eqMgr;
        private QuoteManager qMgr;

        public UserModelManager(IMemoryCache _cache, IConfiguration _config) : base(_cache, _config)
        {
            eqMgr = new EquityManager(_cache, _config);
            qMgr = new QuoteManager(_cache, _config);
        }

        public async Task<ManagerResult<List<UserModel>>> GetModelList(User user)
        {
            ManagerResult<List<UserModel>> mgrResult = new ManagerResult<List<UserModel>>();
           
            try
            {
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    mgrResult.Entity = db.QueryAsync<UserModel>(@" SELECT * 
                                                    FROM user_models 
                                                    WHERE user_id = @p1",
                                                    new { p1 = user.Id }).Result.ToList();
                }
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            } 

            return mgrResult;
        }

        public async Task<ManagerResult<List<ModelEquity>>> GetCore(Guid guid)
        {
            ManagerResult<List<ModelEquity>> mgrResult = new ManagerResult<List<ModelEquity>>();
            try
            {
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    List<ModelEquity> modelEquityList = db.QueryAsync<ModelEquity>(@" SELECT * 
                                                    FROM model_equities 
                                                    WHERE model_id = @p1",
                                                    new { p1 = guid }).Result.ToList();

                    foreach (ModelEquity modelEquity in modelEquityList)
                    {
                        modelEquity.Equity = eqMgr.Get(modelEquity.EquityID).Result.Entity;
                        Quote quote =  qMgr.GetLast(modelEquity.EquityID).Result.Entity;
                        if (quote != null)
                        {
                            modelEquity.LastPrice = quote.Price;
                        }
                    }

                    mgrResult.Entity = modelEquityList;
                }
            }
            catch (Exception ex)
            {
                mgrResult.Exception = ex;
            }

            return mgrResult;
        }

        public async Task<ManagerResult<List<ModelEquity>>> GetCoreByDate(Guid guid, DateTime quoteDate)
        {
            ManagerResult<List<ModelEquity>> mgrResult = new ManagerResult<List<ModelEquity>>();

            try
            {
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    List<ModelEquity> modelEquityList = db.QueryAsync<ModelEquity>(@" SELECT * 
                                                    FROM model_equities 
                                                    WHERE model_id = @p1",
                                                    new { p1 = guid }).Result.ToList();

                    foreach (ModelEquity modelEquity in modelEquityList)
                    {
                        modelEquity.Equity = eqMgr.Get(modelEquity.EquityID).Result.Entity;
                       // Quote quote = qMgr.GetLast(modelEquity.EquityID).Result.Entity;
                        Quote quote = qMgr.GetByEquityIdAndDate(modelEquity.EquityID, quoteDate).Result.Entity;
                        if (quote != null)
                        {
                            modelEquity.LastPrice = quote.Price;
                            modelEquity.LastPriceDate = quoteDate;
                        }
                    }

                    mgrResult.Entity = modelEquityList;
                }
            }
            catch (Exception ex)
            {
                mgrResult.Exception = ex;
            }

            return mgrResult;
        }

        public async Task<ManagerResult<UserModel>> Save(UserModel userModel)
        {
            ManagerResult<UserModel> mgrResult = new ManagerResult<UserModel>();
            
            try
            {
                if (userModel.Id == Guid.Empty)
                {
                    using NpgsqlConnection db = new NpgsqlConnection(connString);
                    {
                        await db.InsertAsync(userModel);
                    }
                }
                else
                {
                    using NpgsqlConnection db = new NpgsqlConnection(connString);
                    {
                        await db.UpdateAsync(userModel);
                    }
                }           
                mgrResult.Entity = userModel;
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            } 

            return mgrResult;
        }

        #region model equities

        public async Task<ManagerResult<ModelEquity>> Get(Guid modelequityId)
        {
            ManagerResult<ModelEquity> mgrResult = new ManagerResult<ModelEquity>();

            try
            {
                using NpgsqlConnection db = new NpgsqlConnection(connString);
                {
                    mgrResult.Entity = await db.QueryFirstOrDefaultAsync<ModelEquity>(@" SELECT * 
                                                            FROM model_equities 
                                                            WHERE id = @p1",
                                                            new { p1 = modelequityId });
                }
            }
            catch (Exception ex)
            {
                mgrResult.Exception = ex;
            }

            return mgrResult;
        }

        public async Task<ManagerResult<List<ModelEquity>>> GetEquityList(UserModel userModel)

        {
            ManagerResult<List<ModelEquity>> mgrResult = new ManagerResult<List<ModelEquity>>();
           
            try
            {
                using NpgsqlConnection db = new NpgsqlConnection(connString);
                {
                    mgrResult.Entity = db.QueryAsync<ModelEquity>(@"SELECT * 
                                                            FROM model_equities 
                                                            WHERE model = @p1 ", 
                                                            new { p1 = userModel.Id } ).Result.ToList();
                }
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            } 

            return mgrResult;
        }

        #region CRUD

        public async Task<ManagerResult<ModelEquity>> AddEquity(Guid userModelId, Guid equityId, int percent)
        {
            ManagerResult<ModelEquity> mgrResult = new ManagerResult<ModelEquity>();           
            
            try
            {   
                ModelEquity modelEquity = new ModelEquity()
                {
                    ModelId = userModelId,
                    EquityID = equityId,
                    Percent = percent
                };

                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    await db.InsertAsync(modelEquity); 
                }                              

                mgrResult.Entity = modelEquity;
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            } 

            return mgrResult;
        }      

        public async Task<ManagerResult<ModelEquity>> Update(Guid modelequityId, Guid userModelId, Guid equityId, int percent)
        {
            ManagerResult<ModelEquity> mgrResult = new ManagerResult<ModelEquity>();

            try
            {
                ModelEquity modelEquity = new ModelEquity()
                {
                    Id = modelequityId,
                    ModelId = userModelId,
                    EquityID = equityId,
                    Percent = percent                    
                };

                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    await db.UpdateAsync(modelEquity);
                }

                mgrResult.Entity = modelEquity;
            }
            catch (Exception ex)
            {
                mgrResult.Exception = ex;
            }

            return mgrResult;
        }


        public async Task<ManagerResult<bool>> RemoveEquity(Guid modelequityId)
        {
            ManagerResult<bool> mgrResult = new ManagerResult<bool>();
            
            try
            {   
                ModelEquity modelEquity = new ModelEquity()
                {
                    Id = modelequityId                    
                };

                using NpgsqlConnection db = new NpgsqlConnection(connString);
                {
                    mgrResult.Entity = await db.DeleteAsync(modelEquity);
                }

            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            } 

            return mgrResult;
        }

        #endregion

    #endregion
    }
}
