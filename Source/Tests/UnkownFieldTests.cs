using NUnit.Framework;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace ProtoSharp.Core
{
    [TestFixture]
    public class UnkownFieldTests
    {
        [Test]
        public void ShouldSupportVarint()
        {
            var field = UnknownField.Create(new MessageTag(1, WireType.Varint),
                new MessageReader(0x96, 1));

            Assert.AreEqual(150, field.Value);
        }
        [Test]
        public void ShouldSupportFixed64()
        {
            var field = UnknownField.Create(new MessageTag(2, WireType.Fixed64),
                new MessageReader(1, 2, 3, 4, 5, 6, 7, 8));

            Assert.AreEqual(0x0807060504030201, field.Value);
        }
        [Test]
        public void ShouldSupportString()
        {
            var message = "Hello World!";
            var bytes = new List<byte>();
            bytes.Add((byte)message.Length);
            bytes.AddRange(Encoding.UTF8.GetBytes(message));
            var field = UnknownField.Create(new MessageTag(3, WireType.String), 
                new MessageReader(bytes.ToArray()));

            Assert.AreEqual(message, field.Value);
        }
        [Test]
        public void ShouldSupportFixed32()
        {
            var field = UnknownField.Create(new MessageTag(4, WireType.Fixed32),
                new MessageReader(3, 2, 1, 0));

            Assert.AreEqual(0x010203, field.Value);
        }
        [Test]
        public void ShouldSupportGroups()
        {
            var input = new MemoryStream();
            var writer = new MessageWriter(input)
                .WriteHeader(1, WireType.Varint)
                .WriteVarint(42)
                .WriteHeader(2, WireType.Fixed32)
                .WriteFixed(0xdeadbeef)
                .WriteHeader(5, WireType.EndGroup);
            input.Position = 0;
            var field = UnknownField.Create(new MessageTag(5, WireType.StartGroup), new MessageReader(input));
            var nested = field.Value as UnknownFieldCollection;

            Assert.AreEqual(2, nested.Count);            
        }
    }
}
