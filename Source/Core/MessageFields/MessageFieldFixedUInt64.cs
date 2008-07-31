using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldFixedUInt64 : MessageFieldFixed<UInt64>
    {
        public MessageFieldFixedUInt64(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Fixed64) { }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadFixedUInt64", Type.EmptyTypes));
        }
    }
}
