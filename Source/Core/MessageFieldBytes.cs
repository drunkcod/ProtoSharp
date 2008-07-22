using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldBytes : MessageField
    {
        public MessageFieldBytes(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Varint) { }

        protected override bool CanAppendWriteCore { get { return true; } }

        public override void AppendWriteField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteBytes", new Type[] { typeof(byte[]) }));
        }
        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadBytes();
        }
    }
}
