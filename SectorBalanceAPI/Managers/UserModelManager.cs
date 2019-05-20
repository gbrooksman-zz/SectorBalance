using System;
using System.Collections.Generic;
using Dapper;
using Npgsql;
using SectorBalanceShared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Dapper.FastCrud;

namespace SectorBalanceAPI
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



        #region model symbols

        public ManagerResult<List<ModelSymbol>> GetSymbolList(UserModel userModel)
        {
            ManagerResult<List<ModelSymbol>> mgrResult = new ManagerResult<List<ModelSymbol>>();
            List<ModelSymbol> modelSymbols = new List<ModelSymbol>();
            
            try
            {  
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    modelSymbols = db.Query<ModelSymbol>("SELECT * FROM model_symbols WHERE model = @m",userModel.Id).ToList();                         
                }

                mgrResult.Entity = modelSymbols;
            }
            catch(Exception ex)
            {
                mgrResult.Entity = default(List<ModelSymbol>);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            } 

            return mgrResult;
        }

      

        public ManagerResult<ModelSymbol> AddSymbol(UserModel userModel, string symbol, int percent)
        {
            ManagerResult<ModelSymbol> mgrResult = new ManagerResult<ModelSymbol>();
            ModelSymbol modelSymbol = new ModelSymbol();
            
            try
            {   
                modelSymbol.ModelId = userModel.Id;

                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    db.Insert(modelSymbol);                               
                }

                mgrResult.Entity = modelSymbol;
            }
            catch(Exception ex)
            {
                mgrResult.Entity = default(ModelSymbol);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            } 

            return mgrResult;
        }

        public ManagerResult<bool> RemoveSymbol(UserModel userModel, string symbol)
        {
            ManagerResult<bool> mgrResult = new ManagerResult<bool>();
            ModelSymbol modelSymbol = new ModelSymbol();
            bool ok = false;
            
            try
            {   
               modelSymbol.ModelId = userModel.Id;
               modelSymbol.Symbol = symbol;

                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    ok = db.Delete(modelSymbol);                             
                }

                mgrResult.Entity = ok;
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
