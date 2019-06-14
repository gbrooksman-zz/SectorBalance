using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SectorBalanceClient.SVG
{
    public interface IBaseElement
    {
        string id { get; set; }
        bool CaptureRef { get; set; }
    }
}