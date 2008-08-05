using System;
using System.Collections.Generic;
using System.Text;

namespace ProtoSharp.Core.UnknownFields
{
    class UnknownFieldString : UnknownField
    {
        public UnknownFieldString(MessageTag tag, MessageReader reader) : base(tag, reader.CreateSubReader().ReadString()) { }

        protected override void SerializeCore(MessageWriter writer)
        {
            writer.WriteString(Value as string);
        }
    }
}
