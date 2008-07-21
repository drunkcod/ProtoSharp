using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldString : MessageField
    {
        public MessageFieldString(int tag, IFieldIO fieldIO): base(tag, fieldIO, WireType.String)
        { }

        public override bool AppendWrite(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteString", new Type[] { typeof(string) }));
            return true;
        }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadString();
        }
        protected override void DoWrite(object value, MessageWriter writer)
        {
            writer.WriteString(value as string);
        }
    }
}
