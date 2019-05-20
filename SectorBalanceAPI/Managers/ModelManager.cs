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
    public class UserModelManager : BaseManager
    {


        public UserModelManager(IMemoryCache _cache, IConfiguration _config) : base(_cache, _config)
        {
            
        }

       public ManagerResult<UserModel> Save(UserModel userModel)
        {
            ManagerResult<UserModel> mgrResult = new ManagerResult<UserModel>();
            
            try
            {
                userModel.Active = !userModel.Active;
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
                mgrResult.ResultEntity = userModel;
            }
            catch(Exception ex)
            {
                mgrResult.ResultEntity = default(UserModel);
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
                mgrResult.ResultEntity = userModel;
            }
            catch(Exception ex)
            {
                mgrResult.ResultEntity = default(UserModel);
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

                mgrResult.ResultEntity = modelSymbols;
            }
            catch(Exception ex)
            {
                mgrResult.ResultEntity = default(List<ModelSymbol>);
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

                mgrResult.ResultEntity = modelSymbol;
            }
            catch(Exception ex)
            {
                mgrResult.ResultEntity = default(ModelSymbol);
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

                mgrResult.ResultEntity = ok;
            }
            catch(Exception ex)
            {
                mgrResult.ResultEntity = false;
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            } 

            return mgrResult;
        }





        #endregion

    }
}
