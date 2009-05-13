using System;
using System.Collections.Generic;
using System.Text;

namespace ProtoSharp.Core
{
    static class GenericExtensions
    {
        public static bool EqualsAny<T>(this T item, params T[] values)
        {
            for (int i = 0; i != values.Length; ++i)
                if (item.Equals(values[i]))
                    return true;
            return false;
        }
    }
}
