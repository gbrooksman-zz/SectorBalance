using Microsoft.AspNetCore.Components;
using SectorBalanceClient.Entities;
using SectorBalanceShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace SectorBalanceClient.Services
{
    public class EquityService
    {

        private readonly AppStateContainer state;
        private readonly HttpClient httpClient;

        public EquityService(HttpClient _httpClient, AppStateContainer _state)
        {
            httpClient = _httpClient;
            state = _state;
        }

        public async Task<List<EquityQuotes>> GetEquitiwsWithQuotes(string EquityList, DateTime StartDate, DateTime StopDate)
        {
            List<Quote> allQuotesList = new List<Quote>();

            List<EquityQuotes> equitiesWithQuotes = new List<EquityQuotes>();

            List<string> quotes = new List<string>(EquityList.Split(";".ToCharArray()));

            allQuotesList = await httpClient.GetJsonAsync<List<Quote>>($@"{state.APIUrl}/api/quote/GetRangeForList
                                                                ?symbols={EquityList}
                                                                &startdate={StartDate}
                                                                &stopdate={StopDate}");

            List<Equity> equities = await httpClient.GetJsonAsync<List<Equity>>($@"{state.APIUrl}/api/equity/GetList?symbols={EquityList}");

            foreach (Equity equity in equities)
            {
                List<Quote> quoteList = allQuotesList.Where(q => q.EquityId == equity.Id)
                                                        .OrderByDescending(q => q.Date)
                                                        .ToList();
                equitiesWithQuotes.Add(new EquityQuotes
                {
                    Equity = equity,
                    Quotes = quoteList
                });
            }

            return equitiesWithQuotes;
        }
    }
}
