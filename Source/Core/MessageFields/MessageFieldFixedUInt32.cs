using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core.MessageFields
{
    class MessageFieldFixedUInt32 : MessageFieldFixed<uint>
    {
        public MessageFieldFixedUInt32(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Fixed32) { }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadFixedUInt32", Type.EmptyTypes));
        }
    }
}
