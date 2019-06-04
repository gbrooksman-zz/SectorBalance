using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SectorBalanceShared;
using System.Net.Http;

namespace SectorBalanceClient.Services
{
    public class EquityService
    {
        HttpClient http;

        public EquityService(HttpClient httpInstance)
        {
            http = httpInstance;
        }

        protected List<Equity> GetEquityList()
        {
            List<Equity> equityList = new List<Equity>(); // = http.GetJsonAsync<List<Equity>>("/api/Equity/GetList").Result;

            return equityList;
        }


    }
}
