using System;
using System.Reflection;

namespace ProtoSharp.Core
{
    class MessageFieldString : MessageField
    {
        public MessageFieldString(int tag, IFieldIO fieldIO): base(tag, fieldIO, WireType.String)
        { }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadString();
        }
        protected override void DoWrite(object value, MessageWriter writer)
        {
            if(value == null)
                value = string.Empty;
            writer.WriteString(value.ToString());
        }
    }
}
