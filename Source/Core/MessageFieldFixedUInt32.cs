using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldFixedUInt32 : MessageFieldFixed<uint>
    {
        public MessageFieldFixedUInt32(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Fixed32) { }

        protected override bool CanAppendReadCore { get { return true; } }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadFixedUInt32", Type.EmptyTypes));
        }
        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadFixedUInt32();
        }
    }
}
