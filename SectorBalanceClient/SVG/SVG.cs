using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SectorBalanceClient.SVG
{
    public class SVG : IBaseElement
    {
        public string id { get; set; } = null;
        public bool CaptureRef { get; set; } = false;
        public double width { get; set; } = double.NaN;
        public double height { get; set; } = double.NaN;
        public string xmlns { get; set; } = null;
        public ICollection<object> Children { get; set; } = new List<object>();
        public string style { get; set; } = null;
        public string fill { get; set; } = null;
        public string viewBox { get; set; } = "0 0 450 420";
        public string transform { get; set; } = null;
        public string onclick { get; set; } = null;
    }
}