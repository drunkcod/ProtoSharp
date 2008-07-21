using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldBytes : MessageField
    {
        public MessageFieldBytes(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Varint) { }

        public override bool AppendWrite(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteBytes", new Type[] { typeof(byte[]) }));
            return true;
        }
        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadBytes();
        }
        protected override void DoWrite(object value, MessageWriter writer)
        {
            writer.WriteBytes((byte[])value);
        }
    }
}
