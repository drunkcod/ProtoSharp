using System.Reflection;
using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core.MessageFields
{
    class MessageFieldUInt64 : MessageFieldVarint<UInt64>
    {
        public MessageFieldUInt64(int tag, IFieldIO fieldIO) : base(tag, fieldIO) { }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadVarint64", Type.EmptyTypes));
        }
    }
}
