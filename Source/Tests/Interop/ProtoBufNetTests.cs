using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ProtoSharp.Tests.Messages;
using ProtoBuf;
using System.IO;

namespace ProtoSharp.Tests.Interop
{
    [TestFixture]
    public class ProtoBufNetTests
    {
        [Test,Ignore("Not supported it seems")]
        public void Deserialize_ShouldFillInDefaultsForEmptyStream()
        {
            Person.PhoneNumber phoneNumber = Serializer.Deserialize<Person.PhoneNumber>(Stream.Null);
            Assert.AreEqual(Person.PhoneType.Home, phoneNumber.Type);
        }
    }
}
