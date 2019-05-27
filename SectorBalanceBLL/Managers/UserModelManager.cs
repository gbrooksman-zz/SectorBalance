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
            List<UserModel> models = new List<UserModel>();
            
            try
            {  
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    models = db.Query<UserModel>("SELECT * FROM user_models WHERE ucer_id = @id",user.Id).ToList();                         
                }

                mgrResult.Entity = models;
            }
            catch(Exception ex)
            {
                mgrResult.Entity = default(List<UserModel>);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
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
                    using (NpgsqlConnection db = new NpgsqlConnection(connString))
                    {
                        db.Insert(userModel);
                    }
                }
                else
                {
                    using (NpgsqlConnection db = new NpgsqlConnection(connString))
                    {
                        db.Update(userModel);
                    }
                }           
                mgrResult.Entity = userModel;
            }
            catch(Exception ex)
            {
                mgrResult.Entity = default(UserModel);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            } 

            return mgrResult;
        }



        public ManagerResult<UserModel> ToggleActive(UserModel userModel)
        {
            ManagerResult<UserModel> mgrResult = new ManagerResult<UserModel>();
            
            try
            {
                userModel.Active = !userModel.Active;
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    bool ok = db.Update(userModel);               
                }
                mgrResult.Entity = userModel;
            }
            catch(Exception ex)
            {
                mgrResult.Entity = default(UserModel);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
           } 
           
           return mgrResult;
        }

        #region model equities

        public ManagerResult<List<ModelEquity>> GetEquityList(UserModel userModel)
        {
            ManagerResult<List<ModelEquity>> mgrResult = new ManagerResult<List<ModelEquity>>();
            List<ModelEquity> modelEquities = new List<ModelEquity>();
            
            try
            {  
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    modelEquities = db.Query<ModelEquity>("SELECT * FROM model_equities WHERE model = @m",userModel.Id).ToList();                         
                }

                mgrResult.Entity = modelEquities;
            }
            catch(Exception ex)
            {
                mgrResult.Entity = default(List<ModelEquity>);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
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
                {
                    db.Insert(modelEquity);                               
                }

                mgrResult.Entity = modelEquity;
            }
            catch(Exception ex)
            {
                mgrResult.Entity = default(ModelEquity);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            } 

            return mgrResult;
        }

        public ManagerResult<bool> RemoveEquity(UserModel userModel, Guid equityId)
        {
            ManagerResult<bool> mgrResult = new ManagerResult<bool>();
            
            try
            {   
                ModelEquity modelEquity = new ModelEquity()
                {
                    ModelId = userModel.Id,
                    EquityID = equityId
                };

                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    mgrResult.Entity = db.Delete(modelEquity);                             
                }

            }
            catch(Exception ex)
            {
                mgrResult.Entity = false;
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            } 

            return mgrResult;
        }





        #endregion

    }
}
