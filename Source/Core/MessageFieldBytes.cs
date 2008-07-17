using System;
using System.Reflection;

namespace ProtoSharp.Core
{
    class MessageFieldBytes : MessageField
    {
        public MessageFieldBytes(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Varint) { }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadBytes();
        }
        protected override void DoWrite(object value, MessageWriter writer)
        {
            writer.WriteBytes((byte[])value);
        }
    }
}
