using System;
using System.Collections.Generic;
using System.Text;

namespace ProtoSharp.Core
{
    public static class UnixTime
    {
        public static readonly DateTime Epoch = new DateTime(1971, 1, 1);

        public static int From(DateTime date)
        {
            return (int)((date - Epoch).Ticks / TimeSpan.TicksPerMillisecond);
        }

        public static DateTime ToDateTime(int unixTime)
        {
            return Epoch.AddMilliseconds(unixTime);
        }
    }
}
