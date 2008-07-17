using System.Reflection;

namespace ProtoSharp.Core
{
    class MessageFieldFixedUInt32 : MessageField
    {
        public MessageFieldFixedUInt32(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Fixed32) { }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadFixedUInt32();
        }
        protected override void DoWrite(object value, MessageWriter writer)
        {
            writer.WriteFixed32((uint)value);
        }
    }
}
