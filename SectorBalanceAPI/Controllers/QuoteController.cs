using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SectorBalanceBLL;
using SectorBalanceShared;
using System.Threading.Tasks;


namespace SectorBalanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private readonly QuoteManager qMgr;
        private readonly EquityManager eMgr;
        private readonly EquityGroupManager egMgr;
        private readonly UserModelManager umMgr;

        public QuoteController(IMemoryCache _cache, IConfiguration _config)
        {
            qMgr = new QuoteManager(_cache, _config);
            eMgr = new EquityManager(_cache, _config);
            egMgr = new EquityGroupManager(_cache, _config);
            umMgr = new UserModelManager(_cache, _config);
        }

        [HttpGet]
        [Route("GetRange")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Quote>>> GetRange(string symbol, DateTime startdate, DateTime stopdate)
        {
            ManagerResult<List<Quote>> mrQuoteList = new ManagerResult<List<Quote>>();
            ManagerResult<Equity> mrEquity = await eMgr.GetBySymbol(symbol);

            if (!mrEquity.Success)
            {
                return BadRequest(mrEquity);
            }
            else
            {
                Equity equity = mrEquity.Entity;
                mrQuoteList = await qMgr.GetByEquityIdAndDateRange(equity.Id, startdate, stopdate);
                if (!mrQuoteList.Success)
                {
                    return BadRequest(mrQuoteList);
                }
            }
            return Ok(mrQuoteList.Entity);
        }

        [HttpGet]
        [Route("GetDate")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Quote>> GetDate(string symbol, DateTime date)
        {
            ManagerResult<Equity> mrEquity = await eMgr.GetBySymbol(symbol);
            ManagerResult<Quote> mrQuoteList;
            if (!mrEquity.Success)
            {
                return BadRequest(mrEquity);
            }
            else
            {
                Equity equity = mrEquity.Entity;
                mrQuoteList = await qMgr.GetByEquityIdAndDate(equity.Id, date);
                if (!mrQuoteList.Success)
                {
                    return BadRequest(mrQuoteList);
                }
            }
            return Ok(mrQuoteList.Entity);
        }

        [HttpGet]
        [Route("GetModelByDate")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ModelEquity>>> GetModelByDate(Guid modelId, DateTime date, int versionNumber)
        {
            ManagerResult<List<ModelEquity>> mrME = await umMgr.GetModelByDate(modelId, date, versionNumber);
            if (!mrME.Success)
            {
                return BadRequest(mrME);
            }
           
            return Ok(mrME.Entity);
        }

        [HttpGet]
        [Route("GetLast")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Quote>> GetLast(Guid equityid)
        {
            ManagerResult<Quote> mrQuoteList = await qMgr.GetLast(equityid);
            if (!mrQuoteList.Success)
            {
                return BadRequest(mrQuoteList);            
            }
            return Ok(mrQuoteList.Entity);
        }

        [HttpGet]
        [Route("GetLastQuoteDate")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DateTime>> GetLastQuoteDate()
        {
            ManagerResult<DateTime> mrLastQuoteDate = await qMgr.GetLastQuoteDate();
            if (!mrLastQuoteDate.Success)
            {
                return BadRequest(mrLastQuoteDate);
            }
            return Ok(mrLastQuoteDate.Entity);
        }



    }
}