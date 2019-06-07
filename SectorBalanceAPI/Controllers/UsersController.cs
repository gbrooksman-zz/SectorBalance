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
    public class UsersController : ControllerBase
    {
        private readonly UserManager userMgr;

        public UsersController(IMemoryCache _cache, IConfiguration _config)
        {
            userMgr = new UserManager(_cache, _config);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult>Get(Guid id)
        {
            var result = await userMgr.GetOneById(id);
            User user = result.Entity;

            if (user == default(User))
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(user);
            }
        }

        [HttpGet("{userName}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(string userName)
        {
           var result = await userMgr.GetOneByName(userName);
           User user = result.Entity;

            if (user == default(User))
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(user);
            }
        }

        [HttpPost()]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Save([FromBody] User user)
        {
            var result = await userMgr.Save(user);
            User newuser = result.Entity;

            if (newuser == default(User))
            {
                return BadRequest(result);
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

        [HttpGet("{validate}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Validate(string username, string password)
        {
            var result = await userMgr.Validate(username, password);
            bool isOK = result.Entity;

            if (!isOK)
            {
                return Unauthorized(result);
            }
            else
            {
                return Ok(isOK);
            }
        }
    }
}