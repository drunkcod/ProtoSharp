using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldUInt32 : MessageFieldVarint<uint>
    {
        public MessageFieldUInt32(int tag, IFieldIO fieldIO) : base(tag, fieldIO) { }

        protected override bool CanAppendReadCore { get { return true; } }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadVarint32", Type.EmptyTypes));
        }

        protected override object DoRead(MessageReader reader)
        {
            return (uint)reader.ReadVarint32();
        }
    }
}
