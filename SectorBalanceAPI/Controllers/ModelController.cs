using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SectorBalanceBLL;
using SectorBalanceShared;
using System.Threading.Tasks;
using Serilog;
using System;

namespace SectorBalanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly UserModelManager umMgr;
        private IConfiguration config;

        public ModelController(IMemoryCache _cache, IConfiguration _config)
        {
            umMgr = new UserModelManager(_cache, _config);
            config = _config;
        }

        [HttpGet]
        [Route("GetModelList")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<UserModel>>> GetModelList(User user)
        {
            ManagerResult<List<UserModel>> mgrResult = await umMgr.GetActiveModelList(user);

            return Ok(mgrResult.Entity);
        }
               

        [HttpGet]
        [Route("GetModel")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserModel>> GetModel(Guid modelId)
        { 
            ManagerResult<UserModel> mgrResult = await umMgr.GetModel(modelId);

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
        [Route("GetModelVersions")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<UserModel>>> GetModelVersions(Guid modelId)
        {
            ManagerResult<List<UserModel>> mgrResult = await umMgr.GetModelVersions(modelId);

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