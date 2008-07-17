using System.Reflection;
using System;

namespace ProtoSharp.Core
{
    class MessageFieldSInt64 : MessageField
    {
        public MessageFieldSInt64(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Varint) { }

        protected override object DoRead(MessageReader reader)
        {
            UInt64 value = (UInt64)reader.ReadVarint64();
            UInt64 mask = 0L - (value & 1);
            return (Int64)(value >> 1 ^ mask);
        }

        protected override void DoWrite(object value, MessageWriter writer)
        {
            Int64 i = (Int64)value;
            writer.WriteVarint(i << 1 ^ i >> 63);
        }
    }
}
