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
    public class UserModelManager : BaseManager
    {
        public UserModelManager(IMemoryCache _cache, IConfiguration _config) : base(_cache, _config)
        {
            
        }

        public ManagerResult<List<UserModel>> GetModelList(User user)
        {
            ManagerResult<List<UserModel>> mgrResult = new ManagerResult<List<UserModel>>();
           
            try
            {
                using NpgsqlConnection db = new NpgsqlConnection(connString);
                mgrResult.Entity = db.Query<UserModel>(@" SELECT * 
                                                    FROM user_models 
                                                    WHERE ucer_id = @p1",
                                                    new { p1 = user.Id }).ToList();
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            } 

            return mgrResult;
        }

       public ManagerResult<UserModel> Save(UserModel userModel)
        {
            ManagerResult<UserModel> mgrResult = new ManagerResult<UserModel>();
            
            try
            {
                if (userModel.Id == Guid.Empty)
                {
                    using NpgsqlConnection db = new NpgsqlConnection(connString);
                    db.Insert(userModel);
                }
                else
                {
                    using NpgsqlConnection db = new NpgsqlConnection(connString);
                    db.Update(userModel);
                }           
                mgrResult.Entity = userModel;
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            } 

            return mgrResult;
        }

        public ManagerResult<UserModel> TogglePrivate(UserModel userModel)
        {
            userModel.IsPrivate = !userModel.IsPrivate;
            return Save(userModel);
        }

        public ManagerResult<UserModel> ToggleActive(UserModel userModel)
        {
            userModel.Active = !userModel.Active;
            return Save(userModel);
        }

        #region model equities

        public ManagerResult<List<ModelEquity>> GetEquityList(UserModel userModel)
        {
            ManagerResult<List<ModelEquity>> mgrResult = new ManagerResult<List<ModelEquity>>();
           
            try
            {
                using NpgsqlConnection db = new NpgsqlConnection(connString);
                mgrResult.Entity = db.Query<ModelEquity>(@"SELECT * 
                                                            FROM model_equities 
                                                            WHERE model = @p1 ", 
                                                            new { p1 = userModel.Id } ).ToList();
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            } 

            return mgrResult;
        }      

        public ManagerResult<ModelEquity> AddEquity(UserModel userModel, Guid equityId, int percent)
        {
            ManagerResult<ModelEquity> mgrResult = new ManagerResult<ModelEquity>();           
            
            try
            {   
                ModelEquity modelEquity = new ModelEquity()
                {
                    ModelId = userModel.Id,
                    EquityID = equityId,
                    Percent = percent
                };

                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                db.Insert(modelEquity);                               

                mgrResult.Entity = modelEquity;
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            } 

            return mgrResult;
        }

        public ManagerResult<ModelEquity> Update(Guid modelequityId, UserModel userModel, Guid equityId, int percent)
        {
            ManagerResult<ModelEquity> mgrResult = new ManagerResult<ModelEquity>();

            try
            {
                ModelEquity modelEquity = new ModelEquity()
                {
                    Id = modelequityId,
                    ModelId = userModel.Id,
                    EquityID = equityId,
                    Percent = percent,
                    
                };

                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                    db.Update(modelEquity);

                mgrResult.Entity = modelEquity;
            }
            catch (Exception ex)
            {
                mgrResult.Exception = ex;
            }

            return mgrResult;
        }


        public ManagerResult<bool> RemoveEquity(Guid modelequityId)
        {
            ManagerResult<bool> mgrResult = new ManagerResult<bool>();
            
            try
            {   
                ModelEquity modelEquity = new ModelEquity()
                {
                    Id = modelequityId                    
                };

                using NpgsqlConnection db = new NpgsqlConnection(connString);
                mgrResult.Entity = db.Delete(modelEquity);

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
    }
}
