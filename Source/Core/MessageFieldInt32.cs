using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldInt32 : MessageFieldVarint<int>
    {
        public MessageFieldInt32(int tag, IFieldIO fieldIO) : base(tag, fieldIO) { }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadVarint32();
        }
        protected override void DoWrite(object value, MessageWriter writer)
        {
            writer.WriteVarint((int)value);
        }
    }
}
