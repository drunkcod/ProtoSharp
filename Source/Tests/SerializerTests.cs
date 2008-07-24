using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ProtoSharp.Tests.Messages;
using ProtoSharp.Core;

namespace ProtoSharp.Tests
{
    [TestFixture]
    public class SerializerTests
    {
        [Test]
        public void CreateDefault_ShouldFindEnumValues()
        {
            var phoneNumber = Serializer.CreateDefault<Person.PhoneNumber>();
            Assert.AreEqual(Person.PhoneType.Home, phoneNumber.Type);
        }

                class Parsable
        {
            public static Parsable Parse(string s)
            {
                return new Parsable(){ _value = s };
            }

            public string Value { get { return _value; } }

            string _value;
        }
        [Test]
        public void CreateDefaultItem_ShouldBeAbleToUseParseMethodIfAvailable()
        {
            Assert.AreEqual("Parsed", Serializer.CreateDefaultItem<Parsable>("Parsed").Value);
        }

        class TryParseable
        {
            public static bool TryParse(string s, out TryParseable result)
            {
                result = new TryParseable() { _value = s };
                return true;
            }

            public string Value { get { return _value; } }

            string _value;
        }
        [Test]
        public void CreateDefaultItem_ShouldBeAbleToUseTryParseMethodIfAvailable()
        {
            Assert.AreEqual("TryParsed", Serializer.CreateDefaultItem<TryParseable>("TryParsed").Value);
        }
        
        class StringConstructable
        {
            public StringConstructable(string s)
            {
                _value = s;
            }

            public string Value { get { return _value; } }

            string _value;
        }
        [Test]
        public void CreateDefaultItem_ShouldBeAbleToUseConstructorTakingString()
        {
            Assert.AreEqual("Constructed", Serializer.CreateDefaultItem<StringConstructable>("Constructed").Value);
        }
        
        enum Option
        {
            Foo, Bar, Baz, xyzzy
        }
        [Test]
        public void CreateDefaultItem_ShouldHandleEnums()
        {
            Assert.AreEqual(Option.xyzzy, Serializer.CreateDefaultItem<Option>("xyzzy"));

        }

        class MessageWithDefaultMember
        {
            [Default("Parsed")]
            public Parsable Parsable { get; set; }
        }
        [Test]
        public void CreateDefault_ShouldFillInDefaultMember()
        {
            var message = Serializer.CreateDefault<MessageWithDefaultMember>();

            Assert.AreEqual("Parsed", message.Parsable.Value);
        }
    }
}
