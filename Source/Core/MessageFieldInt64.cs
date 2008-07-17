using System;
using System.Reflection;

namespace ProtoSharp.Core
{
    class MessageFieldInt64 : MessageField
    {
        public MessageFieldInt64(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Varint) { }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadVarint64();
        }
        protected override void DoWrite(object value, MessageWriter writer)
        {
            writer.WriteVarint((Int64)value);
        }
    }
}
