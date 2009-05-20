using System;
using NUnit.Framework;
using ProtoSharp.Tests.Messages;
using System.IO;
using NUnit.Framework.SyntaxHelpers;

namespace ProtoSharp.Core
{
    [TestFixture]
    public class DateTimeTests
    {
        [Test]
        public void Should_survive_roundtrip()
        {
            var timeStamp = UnixTime.Convert(DateTime.Now);
            var message = new MessageWithDateTime(){ TimeStamp = timeStamp};

            var bytes = new MemoryStream();
            Serializer.Serialize(bytes, message);
            bytes.Position = 0;
            var clone = Serializer.Deserialize<MessageWithDateTime>(bytes);

            Assert.That(clone.TimeStamp, Is.EqualTo(timeStamp));
        }
    }
}
