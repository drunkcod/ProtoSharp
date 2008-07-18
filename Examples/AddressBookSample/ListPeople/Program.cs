using System;
using System.IO;
using ProtoSharp.Core;
using ProtoSharp.Examples.AddressBookSample.Core;

namespace ProtoSharp.Examples.AddressBookSample.ListPeople
{
    class Program
    {
        static int Main(string[] args)
        {
            if(args.Length != 1)
            {
                Console.Error.WriteLine("usage: ListPeople <address book file>");
                return -1;
            }
            AddressBook addressBook = MessageReader.Read<AddressBook>(File.ReadAllBytes(args[0]));
            Print(addressBook);
            return 0;
        }

        static void Print(AddressBook addressBook)
        {
            foreach(Person person in addressBook.Persons)
            {
                Console.WriteLine("Person ID: {0}", person.Id);
                Console.WriteLine(" Name: {0}", person.Name);
                if(!string.IsNullOrEmpty(person.Email))
                    Console.WriteLine(" E-mail address: {0}", person.Email);

                foreach(Person.PhoneNumber phoneNumber in person.Phones)
                {
                    switch(phoneNumber.Type)
                    {
                        case Person.PhoneType.Mobile:
                            Console.Write("  Mobile phone #: ");
                            break;
                        case Person.PhoneType.Home:
                            Console.Write("  Home phone #: ");
                            break;
                        case Person.PhoneType.Work:
                            Console.Write("  Work phone #: ");
                            break;
                    }
                    Console.WriteLine(phoneNumber.Number);
                }
            }
        }
    }
}
