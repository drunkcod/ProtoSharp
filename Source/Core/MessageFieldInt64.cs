using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldInt64 : MessageFieldVarint<Int64>
    {
        public MessageFieldInt64(int tag, IFieldIO fieldIO) : base(tag, fieldIO) { }

        protected override bool CanAppendReadCore { get { return true; } }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadVarint64", Type.EmptyTypes));
        }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadVarint64();
        }
    }
}
