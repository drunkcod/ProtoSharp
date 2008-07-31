using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldDateTime : MessageField
    {
        public MessageFieldDateTime(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Varint) { }

        public override void AppendWriteField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteDateTime", new Type[] { typeof(DateTime) }));
        }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadDateTime", Type.EmptyTypes));
        }
    }
}
