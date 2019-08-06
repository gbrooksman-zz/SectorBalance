using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SectorBalanceClient.Common
{
    public static class Utils
    {
        public static decimal CalcGainLoss(decimal current, decimal cost)
        {
            return current - cost;
        }
    }
}
