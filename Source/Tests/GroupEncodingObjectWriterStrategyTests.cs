using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;
using ProtoSharp.Tests.Messages;

namespace ProtoSharp.Core
{
    [TestFixture]
    public class GroupEncodingObjectWriterStrategyTests
    {
        [Test]
        public void ShouldWriteStartGroup()
        {
            var output = new MemoryStream();
            var writer = new MessageWriter(output, new GroupEncodingObjectWriterStrategy());
            writer.WriteObject(new Test1(), 1);

            output.Position = 0;
            Assert.AreEqual(
                (byte)MessageTag.AsInt(1, WireType.StartGroup),
                output.ReadByte());
        }
        [Test]
        public void ShouldTerminateWithEndGroup()
        {
            var output = new MemoryStream();
            var writer = new MessageWriter(output, new GroupEncodingObjectWriterStrategy());
            writer.WriteObject(new Test1(), 1);

            var bytes = output.ToArray();

            Assert.AreEqual(
                (byte)MessageTag.AsInt(1, WireType.EndGroup),
                bytes[bytes.Length - 1]);
        }
        [Test]
        public void ShouldNotIncludeLength()
        {
            var output = new MemoryStream();
            var writer = new MessageWriter(output, new GroupEncodingObjectWriterStrategy());
            writer.WriteObject(new Test1(), 1);

            var bytes = output.ToArray();

            Assert.AreEqual(
                (byte)MessageTag.AsInt(1, WireType.Varint),
                bytes[1]);
        }
    }
}
