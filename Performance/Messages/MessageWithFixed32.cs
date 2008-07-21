using System;
using System.Runtime.Serialization;
using ProtoBuf;
using ProtoSharp.Core;

namespace ProtoSharp.Performance.Messages
{
    [Serializable, DataContract]
    class MessageWithFixed32
    {
        [Tag(1, UseFixed = true)]
        [ProtoMember(1, Name = "a", IsRequired = true, DataFormat = DataFormat.FixedSize)]
        public int Integer { get; set; }

        [Tag(2, UseFixed = true)]
        [ProtoMember(2, Name = "b", IsRequired = true, DataFormat = DataFormat.FixedSize)]
        public float Float { get; set; }
    }
}
