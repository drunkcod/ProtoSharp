using System;
using System.Runtime.Serialization;
using ProtoBuf;
using ProtoSharp.Core;

namespace ProtoSharp.Performance.Messages
{
    [Serializable, DataContract]
    class MessageWithString
    {
        [Tag(1)]
        [ProtoMember(1, Name = "a", IsRequired = true)]
        public string Value { get; set; }
    }
}
