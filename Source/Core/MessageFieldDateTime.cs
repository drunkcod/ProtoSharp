using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldDateTime : MessageField
    {
        public MessageFieldDateTime(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Varint) { }

        protected override bool CanAppendWriteCore { get { return true; } }
        protected override bool CanAppendReadCore { get { return true; } }

        public override void AppendWriteField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteDateTime", new Type[] { typeof(DateTime) }));
        }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadDateTime", Type.EmptyTypes));
        }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadDateTime();
        }
    }
}
