using System.Collections.Generic;
using ProtoSharp.Core;

namespace ProtoSharp.Examples.AddressBookSample.Core
{
    /*
    message Person {
      required string name = 1;
      required int32 id = 2;        // Unique ID number for this person.
      optional string email = 3;

      enum PhoneType {
        MOBILE = 0;
        HOME = 1;
        WORK = 2;
      }

      message PhoneNumber {
        required string number = 1;
        optional PhoneType type = 2 [default = HOME];
      }

      repeated PhoneNumber phone = 4;
    }
    */
    public class Person
    {
        public enum PhoneType
        {
            Mobile,
            Home,
            Work
        }

        public class PhoneNumber
        {
            [Tag(1)]
            public string Number { get; set; }
            [Tag(2)]
            public PhoneType Type { get; set; }
        }

        [Tag(1)]
        public string Name { get; set; }
        [Tag(2)]
        public int Id { get; set; }
        [Tag(3)]
        public string Email { get; set; }
        [Tag(4)]
        public List<PhoneNumber> Phones { get { return _phones; } }

        List<PhoneNumber> _phones = new List<PhoneNumber>();
    }
}
