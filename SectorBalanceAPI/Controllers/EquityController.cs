using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SectorBalanceShared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SectorBalanceBLL;

namespace SectorBalanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquityController : ControllerBase
    {
        private readonly IMemoryCache cache;
        private readonly IConfiguration config;

        private readonly EquityManager eqMgr;

        public EquityController(IMemoryCache memoryCache, IConfiguration _config)
        {
            cache = memoryCache;
            config = _config;

            eqMgr = new EquityManager(cache, config);
        }

        [HttpGet]
        public List<Equity> GetList()
        {
            return new List<Equity>(); // eqMgr.GetList();
        }


    }
}