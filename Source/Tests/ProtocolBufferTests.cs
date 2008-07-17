using System;
using NUnit.Framework;
using ProtoSharp.Core;
using System.IO;
using ProtoSharp.Tests.Messages;

namespace ProtoSharp.Tests
{
    using MessageWriter = ProtoSharp.Core.MessageWriter;
    [TestFixture]
    public class ProtocolBufferTests
    {
        [Test]
        public void StringRoundTripShouldWorkForShortStrings()
        {
            MemoryStream output = new MemoryStream();
            new MessageWriter(output).WriteString("testing");
            Assert.AreEqual("testing", new MessageReader(output.ToArray()).ReadString());
        }
        [Test]
        public void StringRoundTripShouldWorkForStringsWithMoreThan127Characters()
        {
            string value = new string('*', 129);
            MemoryStream output = new MemoryStream();
            new MessageWriter(output).WriteString(value);
            Assert.AreEqual(value, new MessageReader(output.ToArray()).ReadString());
        }
        [Test]
        public void Int32RoundTripShouldWorkForThreeByteVarints()
        {
            const int value = 0x8000;
            MemoryStream output = new MemoryStream();
            new MessageWriter(output).WriteVarint(value);
            Assert.AreEqual(value, new MessageReader(output.ToArray()).ReadVarint32());
        }        
    }
}
