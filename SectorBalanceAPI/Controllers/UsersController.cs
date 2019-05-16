using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SectorBalanceShared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace SectorBalanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMemoryCache cache;
        private readonly IConfiguration config;

        private UserManager userMgr;

        public UsersController(IMemoryCache memoryCache, IConfiguration _config)
        {
            cache = memoryCache;
            config = _config;

            userMgr = new UserManager(cache, config);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult  Get(string id)
        {
            User user = new User();



            return Ok(user);
        }


    }

}