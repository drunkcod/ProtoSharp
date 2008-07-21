using System;
using System.Runtime.Serialization;
using ProtoBuf;
using ProtoSharp.Core;

namespace ProtoSharp.Performance.Messages
{
    [Serializable, DataContract]
    class MessageWithBytes
    {
        [Tag(1)]
        [ProtoMember(1, Name = "value", IsRequired = true)]
        public byte[] Value { get; set; }
    }
}
