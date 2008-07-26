using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldSInt16 : MessageField
    {
        public MessageFieldSInt16(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Varint) { }

        public override void AppendWriteField(ILGenerator il)
        {
            il.Emit(OpCodes.Conv_I4);
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteZigZag", new Type[] { typeof(int) }));
        }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadZigZag32", Type.EmptyTypes));
        }
    }
}
