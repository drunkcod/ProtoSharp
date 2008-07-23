using System;
using System.Runtime.Serialization;
using ProtoBuf;
using ProtoSharp.Core;
using System.Collections.Generic;

namespace ProtoSharp.Performance.Messages
{
    [Serializable, DataContract]
    class MessageWithRepeatedItem
    {
        [Tag(1)]
        [ProtoMember(1, Name = "Data", IsRequired = true)]
        public List<int> Value { get { return _data; } set { _data = value; } }

        List<int> _data = new List<int>();
    }
}
