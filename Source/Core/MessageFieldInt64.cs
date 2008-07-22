using System;
using System.Reflection;

namespace ProtoSharp.Core
{
    class MessageFieldInt64 : MessageFieldVarint<Int64>
    {
        public MessageFieldInt64(int tag, IFieldIO fieldIO) : base(tag, fieldIO) { }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadVarint64();
        }
    }
}
