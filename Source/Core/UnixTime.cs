using System;
using System.Collections.Generic;
using System.Text;

namespace ProtoSharp.Core
{
    public static class UnixTime
    {
        public static readonly DateTime Epoch = new DateTime(1970, 1, 1);

        public static long From(DateTime date)
        {
            return (date - Epoch).Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static DateTime Convert(DateTime date) { return ToDateTime(From(date)); }

        public static DateTime ToDateTime(long unixTime)
        {
            return Epoch.AddMilliseconds(unixTime);
        }
    }
}
