using System;
using System.Runtime.Serialization;
using ProtoBuf;
using ProtoSharp.Core;

namespace ProtoSharp.Performance.Messages
{
    [Serializable, DataContract]
    class MessageWithInt32
    {
        [Tag(1)]
        [ProtoMember(1, Name = "a", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int Value { get; set; }
    }
}
