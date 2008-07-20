using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using ProtoSharp.Core;
using ProtoBuf;

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
