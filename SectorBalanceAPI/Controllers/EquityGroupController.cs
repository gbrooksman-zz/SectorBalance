using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SectorBalanceBLL;
using SectorBalanceShared;
using System;
using System.Threading.Tasks;

namespace SectorBalanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquityGroupController : ControllerBase
    {
        private readonly EquityGroupManager egMgr;

        public EquityGroupController(IMemoryCache _cache, IConfiguration _config)
        {
            egMgr = new EquityGroupManager(_cache, _config);
        }

        [HttpGet]
        [Route("GetList")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EquityGroup>>> GetList()
        {
            ManagerResult<List<EquityGroup>> mgrResult = await egMgr.GetList();
            if (!mgrResult.Success)
            {
                return BadRequest(mgrResult);
            }
            else
            {
                return Ok(mgrResult.Entity);
            }
        }

        [HttpGet]
        [Route("GetGroupItems")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EquityGroupItem>>> GetGroupItems(Guid equityGrouid)
        {
            ManagerResult<List<EquityGroupItem>> mgrResult = await egMgr.GetGroupItemsList(equityGrouid);
            if (!mgrResult.Success)
            {
                return BadRequest(mgrResult);
            }
            else
            {
                return Ok(mgrResult.Entity);
            }
        }
    }
}