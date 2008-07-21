using System;
using System.Collections.Generic;
using System.Text;
using ProtoSharp.Core;
using ProtoBuf;
using System.Runtime.Serialization;

namespace ProtoSharp.Performance.Messages
{
    [DataContract]
    class MessageWithSInt32
    {
        [Tag(1, UseZigZag = true)]
        [ProtoMember(1, Name = "a", IsRequired = true, DataFormat = DataFormat.ZigZag)]
        public int Value { get; set; }
    }
}
