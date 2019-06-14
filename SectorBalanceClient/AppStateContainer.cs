using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SectorBalanceShared;

namespace SectorBalanceClient
{
    public class AppStateContainer
    {
        public AppStateContainer()
        {

        }

        public string APIUrl { get; set; }
        public string APIKey { get; set; }
        public string UserName { get; set; }
        public Guid UserId { get; set; }
        public List<Equity> EquityList { get; set; }
        public List<UserModel> UserModels { get; set; }
        public DateTime LastQuoteDate { get; set; }






    }
}
