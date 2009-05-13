using System;
using System.Collections.Generic;
using System.Text;

namespace ProtoSharp.Core.UnknownFields
{
    class UnknownFieldVarint : UnknownField
    {
        public UnknownFieldVarint(MessageTag tag, MessageReader reader) : base(tag, reader.ReadInt64()) { }
        public UnknownFieldVarint(MessageTag tag, long value) : base(tag, value) { }

        protected override void SerializeCore(MessageWriter writer)
        {
            writer.WriteVarint((long)Value);
        }
    }
}
