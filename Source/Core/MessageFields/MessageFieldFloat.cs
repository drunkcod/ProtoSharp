using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core.MessageFields
{
    class MessageFieldFloat : MessageFieldFixed<float>
    {
        public MessageFieldFloat(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Fixed32) { }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadFloat", Type.EmptyTypes));
        }
    }
}
