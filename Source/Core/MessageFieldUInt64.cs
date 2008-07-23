using System.Reflection;
using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldUInt64 : MessageFieldVarint<UInt64>
    {
        public MessageFieldUInt64(int tag, IFieldIO fieldIO) : base(tag, fieldIO) { }

        protected override bool CanAppendReadCore { get { return true; } }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadVarint64", Type.EmptyTypes));
        }

        protected override object DoRead(MessageReader reader)
        {
            return (UInt64)reader.ReadVarint64();
        }
    }
}
