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
    public class EquityGroupManager : BaseManager
    {

        public EquityGroupManager(IMemoryCache _cache, IConfiguration _config) : base(_cache, _config)
        {

        }

        public ManagerResult<EquityGroup> Save(EquityGroup symbolGroup)
        {
            ManagerResult<EquityGroup> mgrResult = new ManagerResult<EquityGroup>();
            
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
                mgrResult.Entity = default(EquityGroup);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            } 

            return mgrResult;
        }

        public ManagerResult<EquityGroup> ToggleActive(EquityGroup symbolGroup)
        {
            ManagerResult<EquityGroup> mgrResult = new ManagerResult<EquityGroup>();
            
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
                mgrResult.Entity = default(EquityGroup);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            } 

            return mgrResult;
        }

   #region symbol group items

        public ManagerResult<List<EquityGroupItem>> GetSymbolList(EquityGroup symbolGroup)
        {
            ManagerResult<List<EquityGroupItem>> mgrResult = new ManagerResult<List<EquityGroupItem>>();
            List<EquityGroupItem> symbolGroupItems = new List<EquityGroupItem>();
            
            try
            {  
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    symbolGroupItems = db.Query<EquityGroupItem>("SELECT * FROM symbol_group_items WHERE model = @m",symbolGroup.Id).ToList();                         
                }

                mgrResult.Entity = symbolGroupItems;
            }
            catch(Exception ex)
            {
                mgrResult.Entity = default(List<EquityGroupItem>);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            } 

            return mgrResult;
        }

        public ManagerResult<EquityGroupItem> AddSymbol(EquityGroup symbolGroup, Guid symbolId)
        {
            ManagerResult<EquityGroupItem> mgrResult = new ManagerResult<EquityGroupItem>();
            EquityGroupItem symbolGroupItem = new EquityGroupItem();
            
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
                mgrResult.Entity = default(EquityGroupItem);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            } 

            return mgrResult;
        }

        public ManagerResult<bool> RemoveSymbol(EquityGroup sysmbolGroup, Guid symbolId)
        {
            ManagerResult<bool> mgrResult = new ManagerResult<bool>();
            EquityGroupItem symbolGroupItem = new EquityGroupItem();
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
