using System.Reflection;

namespace ProtoSharp.Core
{
    class MessageFieldInt32 : MessageField
    {
        public MessageFieldInt32(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Varint) { }

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
