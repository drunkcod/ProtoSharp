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
        [ProtoMember(1, Name = "signed", IsRequired = true, DataFormat = DataFormat.FixedSize)]
        public int Signed { get; set; }

        [Tag(2, UseFixed = true)]
        [ProtoMember(1, Name = "unsigned", IsRequired = true, DataFormat = DataFormat.FixedSize)]
        public uint Unsigned { get; set; }

        [Tag(3, UseFixed = true)]
        [ProtoMember(2, Name = "float", IsRequired = true, DataFormat = DataFormat.FixedSize)]
        public float Float { get; set; }
    }
}
