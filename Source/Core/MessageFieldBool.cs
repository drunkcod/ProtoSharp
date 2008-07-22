using System;
using System.Reflection;

namespace ProtoSharp.Core
{
    class MessageFieldBool : MessageFieldVarint<bool>
    {
        public MessageFieldBool(int tag, IFieldIO fieldIO) : base(tag, fieldIO) { }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadVarint32() == 1;
        }
    }
}
