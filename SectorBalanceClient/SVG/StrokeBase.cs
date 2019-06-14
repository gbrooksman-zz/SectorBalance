using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SectorBalanceClient.SVG
{
    public class strokeBase
    {
        public string stroke { get; set; } = null;

        public double stroke_width { get; set; } = double.NaN;
        public strokeLinecap stroke_linecap { get; set; } = strokeLinecap.none;
        public string stroke_dasharray { get; set; } = null;


    }

    public enum strokeLinecap
    {
        none,
        butt,
        round,
        square,

    }
}