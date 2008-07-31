using System;
using System.Collections.Generic;
using System.Text;

namespace ProtoSharp.Core.UnknownFields
{
    class UnknownFieldFixed32 : UnknownField
    {
        public UnknownFieldFixed32(MessageTag tag, MessageReader reader) : base(tag, reader.ReadFixedInt32()) { }

        protected override void SerializeCore(MessageWriter writer)
        {
            writer.WriteFixed((int)Value);
        }
    }
}
