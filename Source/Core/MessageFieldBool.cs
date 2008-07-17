using System;
using System.Reflection;

namespace ProtoSharp.Core
{
    class MessageFieldBool : MessageField
    {
        public MessageFieldBool(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Varint) { }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadVarint32() == 1;
        }
        protected override void DoWrite(object value, MessageWriter writer)
        {
            writer.WriteVarint(Convert.ToInt32(value));
        }
    }
}
