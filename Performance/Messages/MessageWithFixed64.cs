using System;
using System.Runtime.Serialization;
using ProtoBuf;
using ProtoSharp.Core;

namespace ProtoSharp.Performance.Messages
{
    [Serializable, DataContract]
    class MessageWithFixed64
    {
        [Tag(1, UseFixed = true)]
        [ProtoMember(1, Name = "signed", IsRequired = true, DataFormat = DataFormat.FixedSize)]
        public long Signed { get; set; }

        [Tag(2, UseFixed = true)]
        [ProtoMember(2, Name = "unsigned", IsRequired = true, DataFormat = DataFormat.FixedSize)]
        public long Unsigned { get; set; }
        
        [Tag(3, UseFixed = true)]
        [ProtoMember(3, Name = "double", IsRequired = true, DataFormat = DataFormat.FixedSize)]
        public double Double { get; set; }
    }
}
