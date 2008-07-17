using System;
using System.Reflection;

namespace ProtoSharp.Core
{
    class MessageFieldFixedUInt64 : MessageField
    {
        public MessageFieldFixedUInt64(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Fixed32) { }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadFixedUInt64();
        }
        protected override void DoWrite(object value, MessageWriter writer)
        {
            writer.WriteFixed64((UInt64)value);
        }
    }
}
