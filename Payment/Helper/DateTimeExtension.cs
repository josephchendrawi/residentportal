using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResComm.Web.Payment
{
    public static class DateTimeExtension
    {
        public static DateTime ToMalaysiaTime(this DateTime datetime)
        {
            return datetime.AddHours(8);
        }
    }
}