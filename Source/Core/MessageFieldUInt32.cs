using System.Reflection;

namespace ProtoSharp.Core
{
    class MessageFieldUInt32 : MessageField
    {
        public MessageFieldUInt32(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Varint) { }

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
