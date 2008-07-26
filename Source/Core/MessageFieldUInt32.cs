using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldUInt32 : MessageFieldVarint<uint>
    {
        public MessageFieldUInt32(int tag, IFieldIO fieldIO) : base(tag, fieldIO) { }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadVarint32", Type.EmptyTypes));
        }
    }
}
