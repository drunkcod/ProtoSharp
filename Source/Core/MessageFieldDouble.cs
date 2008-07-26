using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldDouble : MessageFieldFixed<double>
    {
        public MessageFieldDouble(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Fixed64) { }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadDouble", Type.EmptyTypes));
        }
    }
}
