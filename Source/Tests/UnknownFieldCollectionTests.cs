using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace ProtoSharp.Core
{
    [TestFixture]
    public class UnknownFieldCollectionTests
    {
        [Test]
        public void ShouldHaveCountZeroWhenCreated()
        {
            Assert.AreEqual(0, new UnknownFieldCollection().Count);
        }
        [Test]
        public void Add_ShouldIncreaseCount()
        {
            var fields = new UnknownFieldCollection();
            fields.Add(new MessageTag(1, WireType.Varint), new MessageReader(42));

            Assert.AreEqual(1, fields.Count);
        }
        [Test]
        public void ShouldContainFieldAfterAdd()
        {
            var fields = new UnknownFieldCollection();
            fields.Add(new MessageTag(1, WireType.Varint), new MessageReader(42));

            Assert.AreEqual(42, fields[0].Value);
        }
        [Test]
        public void ShouldRememberFieldTag()
        {
            var fields = new UnknownFieldCollection();
            fields.Add(new MessageTag(1, WireType.Varint), new MessageReader(42));
            Assert.AreEqual(WireType.Varint, fields[0].WireType);
            Assert.AreEqual(1, fields[0].Number);
        }
        class EmptyMessage { }
        [Test]
        public void ShouldBeAbleToSerializeConsumedBytes()
        {
            var bytes = new MemoryStream();
            new MessageWriter(bytes)
                .WriteHeader(1, WireType.Varint)
                .WriteVarint(0xbeef)
                .WriteHeader(2, WireType.String)
                .WriteString("Hello World!");
            var fields = new UnknownFieldCollection();
            bytes.Position = 0;
            new MessageReader(bytes).Read<EmptyMessage>(fields);

            var output = new MemoryStream();
            fields.Serialize(output);
            Assert.AreEqual(bytes.ToArray(), output.ToArray());
        }
    }
}
