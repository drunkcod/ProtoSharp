using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldUInt32 : MessageFieldVarint<uint>
    {
        public MessageFieldUInt32(int tag, IFieldIO fieldIO) : base(tag, fieldIO) { }

        protected override object DoRead(MessageReader reader)
        {
            return (uint)reader.ReadVarint32();
        }
        protected override void DoWrite(object value, MessageWriter writer)
        {
            writer.WriteVarint((uint)value);
        }
    }
}
