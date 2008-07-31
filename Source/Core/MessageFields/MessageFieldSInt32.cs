using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldSInt32 : MessageField
    {
        public MessageFieldSInt32(int tag, IFieldIO fieldIO): base(tag, fieldIO, WireType.Varint){}

        public override void AppendWriteField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteZigZag", new Type[] { typeof(int) }));
        }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadZigZag32", Type.EmptyTypes));
        }
    }
}
