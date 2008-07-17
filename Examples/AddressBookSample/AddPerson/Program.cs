using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ProtoSharp.Core;
using ProtoSharp.Examples.AddressBookSample.Core;

namespace ProtoSharp.Examples.AddressBookSample.AddPerson
{
    class Program
    {
        static int Main(string[] args)
        {
            if(args.Length != 1)
            {
                Console.Error.WriteLine("usage: AddPerson <address book file>");
                return -1;
            }
            AddressBook addressBook = new AddressBook();
            if(File.Exists(args[0]))
                addressBook = new MessageReader(File.ReadAllBytes(args[0])).Read<AddressBook>();

            addressBook.Persons.Add(PromptForAddress(Console.In, Console.Out));
            new MessageWriter(File.Create(args[0])).WriteMessage(addressBook);
            return 0;
        }

        static Person PromptForAddress(TextReader input, TextWriter output)
        {
            Person person = new Person();

            output.Write("Enter person ID: ");
            person.Id = int.Parse(input.ReadLine());

            output.Write("Enter name: ");
            person.Name = input.ReadLine();

            output.Write("Enter email address (blank for none): ");
            string email = input.ReadLine();
            if(!string.IsNullOrEmpty(email))
                person.Email = email;

            for(; ; )
            {
                output.Write("Enter a phone number (or leave blank to finnish): ");
                string number = input.ReadLine();
                if(string.IsNullOrEmpty(number))
                    break;

                Person.PhoneNumber phoneNumber = Message.CreateDefault<Person.PhoneNumber>();
                phoneNumber.Number = number;
                output.Write("Is this a mobile, home, or work phone? ");
                string type = input.ReadLine();
                if(type == "mobile")
                    phoneNumber.Type = Person.PhoneType.Mobile;
                else if(type == "home")
                    phoneNumber.Type = Person.PhoneType.Home;
                else if(type == "work")
                    phoneNumber.Type = Person.PhoneType.Work;
                else
                    output.WriteLine("Unknown phone type. Using default.");

                person.Phones.Add(phoneNumber);
            }

            return person;
        }
    }
}
