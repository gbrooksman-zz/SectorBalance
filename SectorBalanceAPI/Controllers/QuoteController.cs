using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SectorBalanceBLL;
using SectorBalanceShared;
namespace SectorBalanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private readonly QuoteManager qMgr;
        private readonly EquityManager eMgr;

        public QuoteController(IMemoryCache _cache, IConfiguration _config)
        {
            qMgr = new QuoteManager(_cache, _config);
            eMgr = new EquityManager(_cache, _config);
        }

        [HttpGet]
        [Route("GetRange")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<Quote>> GetRange(string symbol, DateTime startdate, DateTime stopdate)
        {
            ManagerResult<List<Quote>> mrQuoteList = new ManagerResult<List<Quote>>();
            ManagerResult<Equity> mrEquity = eMgr.GetBySymbol(symbol);

            if (!mrEquity.Success)
            {
                return BadRequest();
            }
            else
            {
                Equity equity = mrEquity.Entity;
                mrQuoteList = qMgr.GetByEquityIdAndDateRange(equity.Id, startdate, stopdate);
                if (!mrQuoteList.Success)
                {
                    return BadRequest();
                }
            }
            return Ok(mrQuoteList.Entity);
        }

        [HttpGet]
        [Route("GetDate")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Quote> GetDate(string symbol, DateTime date)
        {
            ManagerResult<Quote> mrQuoteList = new ManagerResult<Quote>();
            ManagerResult<Equity> mrEquity = eMgr.GetBySymbol(symbol);

            if (!mrEquity.Success)
            {
                return BadRequest();
            }
            else
            {
                Equity equity = mrEquity.Entity;
                mrQuoteList = qMgr.GetByEquityIdAndDate(equity.Id, date);
                if (!mrQuoteList.Success)
                {
                    return BadRequest();
                }
            }
            return Ok(mrQuoteList.Entity);
        }
    }
}