using System;
using System.Reflection;

namespace ProtoSharp.Core
{
    class MessageFieldFixedInt64 : MessageFieldFixed<Int64>
    {
        public MessageFieldFixedInt64(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Fixed32) { }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadFixedInt64();
        }
        protected override void DoWrite(object value, MessageWriter writer)
        {
            writer.WriteFixed((Int64)value);
        }
    }
}
