using System.Reflection;
using System;

namespace ProtoSharp.Core
{
    class MessageFieldUInt64 : MessageFieldVarint<UInt64>
    {
        public MessageFieldUInt64(int tag, IFieldIO fieldIO) : base(tag, fieldIO) { }

        protected override object DoRead(MessageReader reader)
        {
            return (UInt64)reader.ReadVarint64();
        }
    }
}
