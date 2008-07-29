using System;
using System.Collections.Generic;
using System.Text;

namespace ProtoSharp.Core.UnknownFields
{
    class UnknownFieldGroup : UnknownField
    {
        public UnknownFieldGroup(MessageTag tag, MessageReader reader) : base(tag, ReadGroup(tag, reader)) { }

        static UnknownFieldCollection ReadGroup(MessageTag startTag, MessageReader reader)
        {
            var group = new UnknownFieldCollection();
            for(int stop = startTag.WithWireType(WireType.EndGroup), tag = reader.ReadVarint32(); tag != stop; tag = reader.ReadVarint32())
                group.Add(new MessageTag(tag), reader);
            return group;
        }
    }
}
