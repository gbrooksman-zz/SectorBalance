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
        public IActionResult  Get(Guid id)
        {
            User user = userMgr.GetOneById(id).Entity;
            if (user == default(User))
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }
        }

        [HttpGet("{userName}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(string userName)
        {
            User user = userMgr.GetOneByName(userName).Entity;
            if (user == default(User))
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }
        }

        [HttpPost()]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Save([FromBody] User user)
        {
            User newuser = userMgr.Save(user).Entity;
            if (newuser == default(User))
            {
                return NotFound();
            }
            else if (newuser.Id != user.Id)
            {
                return Created("",user.Id);
            }
            else
            {
                return Ok(user);
            }
        }


    }

}