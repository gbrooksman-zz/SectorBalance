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
    public class EquityGroupManager : BaseManager
    {
        // this manager holds methods for equity_groups and equity_group_items tables
        // equity groups are collections of equities that are related by some criteria
        // these are different from user models which are collections of equities created
        // by users for whatever purposed
        
        public EquityGroupManager(IMemoryCache _cache, IConfiguration _config) : base(_cache, _config)
        {

        }

        public async Task<ManagerResult<List<EquityGroup>>> GetList()
        {
            ManagerResult<List<EquityGroup>> mgrResult = new ManagerResult<List<EquityGroup>>();
             
            try
            {
                List<EquityGroup> equityGroupList = await GetAllGroups();
                mgrResult.Entity = equityGroupList;
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            } 

            return mgrResult;
        }
        
        public async Task<ManagerResult<List<EquityGroup>>> GetActiveList()
        {
            ManagerResult<List<EquityGroup>> mgrResult = new ManagerResult<List<EquityGroup>>();           

            try
            {
                List<EquityGroup> equityGroupList =  await GetAllGroups() ;
                mgrResult.Entity = equityGroupList.Where(e => e.Active == true).ToList();
            }
            catch (Exception ex)
            {
                mgrResult.Exception = ex;
            }

            return mgrResult;
        }

        private async Task<List<EquityGroup>> GetAllGroups()
        {
            return await cache.GetOrCreateAsync<List<EquityGroup>>(CacheKeys.EQUITY_GROUP_LIST, entry =>
            {
                using NpgsqlConnection db = new NpgsqlConnection(connString);
                return Task.FromResult(db.Query<EquityGroup>("SELECT * FROM equity_groups").ToList());
            });
        }


        public async Task<ManagerResult<EquityGroup>> Save(EquityGroup equityGroup)
        {
            ManagerResult<EquityGroup> mgrResult = new ManagerResult<EquityGroup>();
            
            try
            {               
                if (equityGroup.Id == Guid.Empty)
                {
                    using NpgsqlConnection db = new NpgsqlConnection(connString);
                    await db.InsertAsync(equityGroup);
                }
                else
                {
                    using NpgsqlConnection db = new NpgsqlConnection(connString);
                    await db.UpdateAsync(equityGroup);
                }           
                mgrResult.Entity = equityGroup;
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            } 

            return mgrResult;
        }

   #region equity group items

        public async Task<ManagerResult<List<EquityGroupItem>>> GetGroupItemsList(Guid equityGroupId)
        {
            ManagerResult<List<EquityGroupItem>> mgrResult = new ManagerResult<List<EquityGroupItem>>();
            List<EquityGroupItem> equityGroupItems = new List<EquityGroupItem>();
     
            try
            {
                equityGroupItems = await cache.GetOrCreateAsync<List<EquityGroupItem>>(equityGroupId, entry =>
                {
                    using NpgsqlConnection db = new NpgsqlConnection(connString);
                    return Task.FromResult(db.Query<EquityGroupItem>(@"SELECT * 
                                                        FROM equity_group_items 
                                                        WHERE group_id = @p1 ", new { p1 = equityGroupId }).ToList());
                });

                mgrResult.Entity = equityGroupItems;
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            } 

            return mgrResult;
        }

        public async Task<ManagerResult<EquityGroupItem>> AddEquity(EquityGroup equityGroup, Guid equityId)
        {
            ManagerResult<EquityGroupItem> mgrResult = new ManagerResult<EquityGroupItem>();
            EquityGroupItem equityGroupItem = new EquityGroupItem();
            
            try
            {   
                equityGroupItem.GroupId = equityGroup.Id;
                equityGroupItem.EquityId = equityId;

                using (NpgsqlConnection db = new NpgsqlConnection(connString))
                {
                    await db.InsertAsync(equityGroupItem);
                }

                mgrResult.Entity = equityGroupItem;
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            } 

            return mgrResult;
        }

        public async Task<ManagerResult<bool>> RemoveEquity(EquityGroup sysmbolGroup, Guid equitylId)
        {
            ManagerResult<bool> mgrResult = new ManagerResult<bool>();

            try
            {
                var equityGroupItem = new EquityGroupItem();
                equityGroupItem.GroupId = sysmbolGroup.Id;
                equityGroupItem.EquityId = equitylId;

                using NpgsqlConnection db = new NpgsqlConnection(connString);
                mgrResult.Entity = await db.DeleteAsync(equityGroupItem);
            }
            catch(Exception ex)
            {
                mgrResult.Exception = ex;
            } 

            return mgrResult;
        }

        #endregion
    }
}
