using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core.MessageFields
{
    class MessageFieldSInt64 : MessageField
    {
        public MessageFieldSInt64(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Varint) { }

        public override void AppendWriteField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteZigZag", new Type[] { typeof(Int64) }));
        }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadZigZag64", Type.EmptyTypes));
        }
    }
}
