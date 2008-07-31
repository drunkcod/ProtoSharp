using System;
using System.Collections.Generic;
using System.Text;

namespace ProtoSharp.Core.UnknownFields
{
    class UnknownFieldFixed64 : UnknownField
    {
        public UnknownFieldFixed64(MessageTag tag, MessageReader reader) : base(tag, reader.ReadFixedInt64()) { }

        protected override void SerializeCore(MessageWriter writer)
        {
            writer.WriteFixed((long)Value);
        }
    }
}
