using System;
using System.Collections.Generic;
using System.Text;

namespace ProtoSharp.Core
{
    public class GroupEncodingObjectWriterStrategy : IObjectWriterStrategy
    {
        public void Write<T>(MessageWriter target, int number, T value) where T : class
        {
            target.WriteHeader(number, WireType.StartGroup);
            Serializer.Serialize(target, value);
            target.WriteHeader(number, WireType.EndGroup);
        }
    }
}
