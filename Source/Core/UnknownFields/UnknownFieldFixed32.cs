using System;
using System.Collections.Generic;
using System.Text;

namespace ProtoSharp.Core.UnknownFields
{
    class UnknownFieldFixed32 : UnknownField
    {
        public UnknownFieldFixed32(MessageTag tag, MessageReader reader) : base(tag, reader.ReadFixedInt32()) { }
    }
}
