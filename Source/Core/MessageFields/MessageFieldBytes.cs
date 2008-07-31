using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldBytes : MessageField
    {
        public MessageFieldBytes(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Varint) { }

        public override void AppendWriteField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteBytes", new Type[] { typeof(byte[]) }));
        }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadBytes", Type.EmptyTypes));
        }
    }
}
