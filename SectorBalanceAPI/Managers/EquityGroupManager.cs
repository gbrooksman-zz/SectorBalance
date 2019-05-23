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

        public ManagerResult<List<EquityGroup>> GetList()
        {
            ManagerResult<List<EquityGroup>> mgrResult = new ManagerResult<List<EquityGroup>>();
            List<EquityGroup> equityGroupList = new List<EquityGroup>();
            
            try
            {  
                equityGroupList = cache.GetOrCreate<List<EquityGroup>>(CacheKeys.EQUITY_GROUP_LIST, entry =>
                {
                    using (NpgsqlConnection db = new NpgsqlConnection(connString))
                    {                    
                        return db.Query<EquityGroup>("SELECT * FROM equity_groups").ToList();
                    }
                });


                mgrResult.Entity = equityGroupList;
            }
            catch(Exception ex)
            {
                mgrResult.Entity = default(List<EquityGroup>);
                mgrResult.Exception = ex;
                mgrResult.Success = false;
                mgrResult.Message = ex.Message;
            } 

            return mgrResult;
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

   #region equity group items

        public ManagerResult<List<EquityGroupItem>> GetGroupItemsList(EquityGroup equityGroup)
        {
            ManagerResult<List<EquityGroupItem>> mgrResult = new ManagerResult<List<EquityGroupItem>>();
            List<EquityGroupItem> equityGroupItems = new List<EquityGroupItem>();
     
            try
            {  
                equityGroupItems = cache.GetOrCreate<List<EquityGroupItem>>(equityGroup.Id, entry =>
                {
                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    return db.Query<EquityGroupItem>(@"SELECT * 
                                                        FROM equity_group_items 
                                                        WHERE group_id = @g",equityGroup.Id).ToList();                         
                }});

                mgrResult.Entity = equityGroupItems;
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
