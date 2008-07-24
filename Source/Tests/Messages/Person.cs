using System;
using System.Collections.Generic;
using System.Text;
using ProtoSharp.Core;
using ProtoBuf;

namespace ProtoSharp.Tests.Messages
{
    class Person
    {
        public enum PhoneType { Mobile, Home, Work }

        [ProtoContract]
        public class PhoneNumber
        {
            [Tag(1)]
            [ProtoMember(1)]
            public string Number { get; set; }
            [Tag(2), Default("Home")]
            [ProtoMember(2)]
            public PhoneType Type { get; set; }
        }
    }
}
