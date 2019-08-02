using System;
using System.Collections.Generic;
using System.Text;

namespace SectorBalanceBLL.Exceptions
{
    public class DateRangeException : Exception
    {
        public DateRangeException()
        {
        }

        public DateRangeException(string message)
            : base(message)
        {
        }

        public DateRangeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
