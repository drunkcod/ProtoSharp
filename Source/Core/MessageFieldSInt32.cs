using System.Reflection;

namespace ProtoSharp.Core
{
    class MessageFieldSInt32 : MessageField
    {
        public MessageFieldSInt32(int tag, IFieldIO fieldIO): base(tag, fieldIO, WireType.Varint){}

        protected override object DoRead(MessageReader reader)
        {
            uint value = (uint)reader.ReadVarint32();
            uint mask = 0 - (value & 1);
            return (int)(value >> 1 ^ mask);
        }

        protected override void DoWrite(object value, MessageWriter writer)
        {
            int i = (int)value;
            writer.WriteVarint(i << 1 ^ i >> 31);
        }
    }
}
