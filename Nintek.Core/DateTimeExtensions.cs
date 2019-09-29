using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core
{
    public static class DateTimeExtensions
    {
        public static DateTime AsUniversalTime(this DateTime dateTime)
        {
            return new DateTime(dateTime.Ticks, DateTimeKind.Utc);
        }
    }
}
