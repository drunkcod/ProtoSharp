using System.Collections.Generic;
using ProtoSharp.Core;
using System.Runtime.Serialization;
using ProtoBuf;

namespace ProtoSharp.Performance.Messages
{
    [DataContract]
    class Person
    {
        [DataContract]
        public enum PhoneType
        {
            Mobile,
            Home,
            Work
        }

        [DataContract]
        public class PhoneNumber
        {
            [Tag(1)]
            [ProtoMember(1, Name = "name", IsRequired = true)]
            public string Number { get; set; }
            [Tag(2), Default("Home")]
            [ProtoMember(2, Name = "type", IsRequired = true)]
            public PhoneType Type { get; set; }
        }

        [Tag(1)]
        [ProtoMember(1, Name = "name", IsRequired = true)]
        public string Name { get; set; }
        [Tag(2)]
        [ProtoMember(2, Name = "id", IsRequired = true)]
        public int Id { get; set; }
        [Tag(3)]
        [ProtoMember(3, Name = "email", IsRequired = true)]
        public string Email { get; set; }
        [Tag(4)]
        [ProtoMember(4, Name = "phones", IsRequired = true)]
        public List<PhoneNumber> Phones { get { return _phones; } }

        List<PhoneNumber> _phones = new List<PhoneNumber>();
    }
}
