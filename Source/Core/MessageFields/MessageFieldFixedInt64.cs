using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtoSharp.Core.MessageFields
{
    class MessageFieldFixedInt64 : MessageFieldFixed<Int64>
    {
        public MessageFieldFixedInt64(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Fixed64) { }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadFixedInt64", Type.EmptyTypes));
        }
    }
}
