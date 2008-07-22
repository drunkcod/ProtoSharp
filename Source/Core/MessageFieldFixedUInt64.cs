using System;
using System.Reflection;

namespace ProtoSharp.Core
{
    class MessageFieldFixedUInt64 : MessageFieldFixed<UInt64>
    {
        public MessageFieldFixedUInt64(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Fixed32) { }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadFixedUInt64();
        }
    }
}
