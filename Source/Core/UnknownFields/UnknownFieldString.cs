using System;
using System.Collections.Generic;
using System.Text;

namespace ProtoSharp.Core.UnknownFields
{
    class UnknownFieldString : UnknownField
    {
        public UnknownFieldString(MessageTag tag, MessageReader reader) : base(tag, reader.CreateSubReader(reader.ReadVarint32()).ReadString()) { }
    }
}
