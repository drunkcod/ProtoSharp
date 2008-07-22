using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldFixedInt32 : MessageFieldFixed<int>
    {
        public MessageFieldFixedInt32(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Fixed32) { }

        protected override bool CanAppendReadCore { get { return true; } }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadFixedInt32", Type.EmptyTypes));
        }
        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadFixedInt32();
        }
    }
}
