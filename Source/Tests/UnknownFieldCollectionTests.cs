using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

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
    }
}
