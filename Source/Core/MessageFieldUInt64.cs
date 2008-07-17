using System.Reflection;
using System;

namespace ProtoSharp.Core
{
    class MessageFieldUInt64 : MessageField
    {
        public MessageFieldUInt64(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Varint) { }

        protected override object DoRead(MessageReader reader)
        {
            return (UInt64)reader.ReadVarint64();
        }
        protected override void DoWrite(object value, MessageWriter writer)
        {
            writer.WriteVarint((UInt64)value);
        }
    }
}
