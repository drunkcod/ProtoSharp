using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace ProtoSharp.Core.MessageWriterTests
{
    [TestFixture]
    public class WriteStringTests
    {
        [Test]
        public void ShouldHandleMultibyteCharacters()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteString("ä");
            Assert.AreEqual(Encoding.UTF8.GetByteCount("ä"), output.ToArray()[0]);
        }

        class MessageWithString
        {
            [Tag(1)]
            public string Value { get; set; }
        }
        [Test]
        public void ShouldTreatNullStringAsEmpty()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteMessage(new MessageWithString());
            Assert.AreEqual(new byte[0], output.ToArray());
        }
        [Test]
        public void ShouldHanldeEmptyString()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteMessage(new MessageWithString() { Value = string.Empty });
            Assert.AreEqual(new byte[] { 1 << 3 | (byte)WireType.String, 0 }, output.ToArray());
        }
    }
}
