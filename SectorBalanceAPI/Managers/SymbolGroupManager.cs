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
    public class SymbolGroupManager : BaseManager
    {

        public SymbolGroupManager(IMemoryCache _cache, IConfiguration _config) : base(_cache, _config)
        {

        }

        public ManagerResult<SymbolGroup> Save(SymbolGroup symbolGroup)
        {
            ManagerResult<SymbolGroup> mgrResult = new ManagerResult<SymbolGroup>();
            
            try
            {               
                if (symbolGroup.Id == Guid.Empty)
                {
                    using (NpgsqlConnection db = new NpgsqlConnection(connString))
                    {
                        db.Insert(symbolGroup);
                    }
                }
                else
                {
                    using (NpgsqlConnection db = new NpgsqlConnection(connString))
                    {
                        db.Update(symbolGroup);
                    }
                }           
                mgrResult.Entity = symbolGroup;
            }
            catch(Exception ex)
            {
                mgrResult.Entity = default(SymbolGroup);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            } 

            return mgrResult;
        }

        public ManagerResult<SymbolGroup> ToggleActive(SymbolGroup symbolGroup)
        {
            ManagerResult<SymbolGroup> mgrResult = new ManagerResult<SymbolGroup>();
            
            try
            {
                symbolGroup.Active = !symbolGroup.Active;
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    bool ok = db.Update(symbolGroup);               
                }
                mgrResult.Entity = symbolGroup;
            }
            catch(Exception ex)
            {
                mgrResult.Entity = default(SymbolGroup);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            } 

            return mgrResult;
        }

   #region symbol group items

        public ManagerResult<List<SymbolGroupItem>> GetSymbolList(SymbolGroup symbolGroup)
        {
            ManagerResult<List<SymbolGroupItem>> mgrResult = new ManagerResult<List<SymbolGroupItem>>();
            List<SymbolGroupItem> symbolGroupItems = new List<SymbolGroupItem>();
            
            try
            {  
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    symbolGroupItems = db.Query<SymbolGroupItem>("SELECT * FROM symbol_group_items WHERE model = @m",symbolGroup.Id).ToList();                         
                }

                mgrResult.Entity = symbolGroupItems;
            }
            catch(Exception ex)
            {
                mgrResult.Entity = default(List<SymbolGroupItem>);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            } 

            return mgrResult;
        }

        public ManagerResult<SymbolGroupItem> AddSymbol(SymbolGroup symbolGroup, Guid symbolId)
        {
            ManagerResult<SymbolGroupItem> mgrResult = new ManagerResult<SymbolGroupItem>();
            SymbolGroupItem symbolGroupItem = new SymbolGroupItem();
            
            try
            {   
                symbolGroupItem.GroupId = symbolGroup.Id;
                symbolGroupItem.SymbolId = symbolId;

                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    db.Insert(symbolGroupItem);                               
                }

                mgrResult.Entity = symbolGroupItem;
            }
            catch(Exception ex)
            {
                mgrResult.Entity = default(SymbolGroupItem);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            } 

            return mgrResult;
        }

        public ManagerResult<bool> RemoveSymbol(SymbolGroup sysmbolGroup, Guid symbolId)
        {
            ManagerResult<bool> mgrResult = new ManagerResult<bool>();
            SymbolGroupItem symbolGroupItem = new SymbolGroupItem();
            bool ok = false;
            
            try
            {   
                symbolGroupItem.GroupId = sysmbolGroup.Id;
                symbolGroupItem.SymbolId = symbolId;

                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    ok = db.Delete(symbolGroupItem);                             
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
