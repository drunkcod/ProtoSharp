using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldInt32 : MessageField
    {
        public MessageFieldInt32(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Varint) { }

        public override bool AppendWrite(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteVarint", new Type[] { typeof(int) }));
            return true;
        }

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
