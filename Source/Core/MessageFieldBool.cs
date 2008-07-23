using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldBool : MessageFieldVarint<bool>
    {
        public MessageFieldBool(int tag, IFieldIO fieldIO) : base(tag, fieldIO) { }

        protected override bool CanAppendReadCore { get { return true; } }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadBool", Type.EmptyTypes));
        }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadVarint32() == 1;
        }
    }
}
