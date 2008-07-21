using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldSInt32 : MessageField
    {
        public MessageFieldSInt32(int tag, IFieldIO fieldIO): base(tag, fieldIO, WireType.Varint){}

        public override bool AppendWrite(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteZigZag", new Type[] { typeof(int) }));
            return true;
        }

        protected override object DoRead(MessageReader reader)
        {
            uint value = (uint)reader.ReadVarint32();
            uint mask = 0 - (value & 1);
            return (int)(value >> 1 ^ mask);
        }

        protected override void DoWrite(object value, MessageWriter writer)
        {
            writer.WriteZigZag((int)value);
        }
    }
}
