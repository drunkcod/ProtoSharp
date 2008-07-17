using System.Collections.Generic;
using ProtoSharp.Core;

namespace ProtoSharp.Examples.AddressBookSample.Core
{
    /*
    // Our address book file is just one of these.
    message AddressBook {
      repeated Person person = 1;
    }
    */
    public class AddressBook
    {
        [Tag(1)]
        public List<Person> Persons { get { return _persons; } }

        List<Person> _persons = new List<Person>();
    }
}
