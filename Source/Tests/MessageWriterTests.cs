using System;
using NUnit.Framework;
using System.IO;
using ProtoSharp.Core;

namespace ProtoSharp.Tests
{
    using MessageWriter = ProtoSharp.Core.MessageWriter;
    using ProtoSharp.Tests.Messages;

    [TestFixture]
    public class MessageWriterTests
    {
        [Test]
        public void WriteVarint_ShouldStore32BitMinusOneInTenBytes()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteVarint(-1);
            Assert.AreEqual(10, output.Length);
        }
        [Test]
        public void WriteVarint_ShouldStore150InTwoBytes()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteVarint(150);
            Assert.AreEqual(new byte[] { 0x96, 0x01 }, output.ToArray());
        }
        [Test]
        public void WriteVarint_ShouldStoreSingleByte42()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteVarint(42);
            Assert.AreEqual(new byte[] { 42 }, output.ToArray());
        }
        [Test]
        public void WriteVarint_ShouldStoreSingleByteFor42L()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteVarint(42L);
            Assert.AreEqual(new byte[] { 42 }, output.ToArray());
        }
        [Test]
        public void WriteVarint_ShouldStore0x8000InThreeBytes()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteVarint(0x8000);
            Assert.AreEqual(new byte[] { 0x80, 0x80, 2 }, output.ToArray());
        }
        [Test]
        public void WriteVarint_ShouldStoreNineBytesForMaxInt64()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteVarint(Int64.MaxValue);
            Assert.AreEqual(9, output.ToArray().Length);
        }
        [Test]
        public void WriteString_ShouldStoreByteCountFirst()
        {
            string s = "testing";
            var output = new MemoryStream();
            new MessageWriter(output).WriteString(s);
            Assert.AreEqual(s.Length, new MessageReader(output.ToArray()).ReadVarint32());
        }
        [Test]
        public void WriteString_ShouldHandleStringsLongerThan127Characters()
        {
            string s = new string('*', 129);
            var output = new MemoryStream();
            new MessageWriter(output).WriteString(s);
            Assert.AreEqual(s.Length, new MessageReader(output.ToArray()).ReadVarint32());
        }
        [Test]
        public void WriteMessage_Test1SimpleMessage()
        {
            var output = new MemoryStream();
            Test1 data = new Test1();
            data.A = 150;
            new MessageWriter(output).WriteMessage(data);
            Assert.AreEqual(EncodingTests.SimpleMessage, output.ToArray());
        }
        [Test]
        public void WriteMessage_Test1Ex64SimpleMessage()
        {
            var output = new MemoryStream();
            var message = new Test1Ex64();
            message.A = 150;
            new MessageWriter(output).WriteMessage(message);
            Assert.AreEqual(EncodingTests.SimpleMessage, output.ToArray());
        }
        [Test]
        public void WriteMessage_Test2WithString()
        {
            var output = new MemoryStream();
            Test2 data = new Test2();
            data.B = "testing";
            new MessageWriter(output).WriteMessage(data);
            Assert.AreEqual(EncodingTests.Test2Testing, output.ToArray());
        }
        [Test]
        public void WriteMessage_Test3EmbeddedMessage()
        {
            var output = new MemoryStream();
            Test3 data = new Test3();
            data.C = new Test1();
            data.C.A = 150;
            new MessageWriter(output).WriteMessage(data);
            Assert.AreEqual(EncodingTests.Test3EmbeddedMessage, output.ToArray());
        }
        [Test]
        public void WriteMessage_WithRepeatedTest1()
        {
            var message = new WithRepeatedTest1();
            message.Data.Add(new Test1() { A = 42 });
            message.Data.Add(new Test1() { A = 0xbeef });

            var expected = new MemoryStream();
            var writer = new MessageWriter(expected);
            var embedded = new MemoryStream();
            var embeddedWriter = new MessageWriter(embedded);

            embeddedWriter.WriteMessage(message.Data[0]);
            writer.WriteHeader(2, WireType.String);
            writer.WriteBytes(embedded.ToArray());

            embedded.SetLength(0);
            embeddedWriter.WriteMessage(message.Data[1]);
            writer.WriteHeader(2, WireType.String);
            writer.WriteBytes(embedded.ToArray());
            Assert.AreEqual(expected.ToArray(), MessageWriter.Write(message));

        }
        [Test]
        public void WriteMessage_WithSInt32MinusOneShouldBeSingleDataByte()
        {
            var message = new WithVarint32();
            message.SInt32 = -1;
            Assert.AreEqual(4, MessageWriter.Write(message).Length);
        }
        [Test]
        public void WriteMessage_WithSInt32MinusOneShouldBeOne()
        {
            var message = new WithVarint32();
            message.SInt32 = -1;
            Assert.AreEqual(1, MessageWriter.Write(message)[1]);
        }
        [Test]
        public void WriteMessage_WithFixed32ShouldUseFiveBytesPerField()
        {
            var message = new WithFixed32();
            Assert.AreEqual(5 * Message.CountFields(message),
                MessageWriter.Write(message).Length);
        }
        [Test]
        public void WriteMessage_WithFixed64ShouldUseNineBytesPerField()
        {
            var message = new WithFixed64();
            Assert.AreEqual(9 * Message.CountFields(message),
                MessageWriter.Write(message).Length);
        }
        [Test]
        public void WriteFixed32_ShouldPutLeastSignificantBitsFirst()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteFixed32(0x04030201);
            Assert.AreEqual(new byte[] { 1, 2, 3, 4 }, output.ToArray());
        }
        [Test]
        public void WriteFixed32_ShouldHandleUnsigned()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteFixed32((uint)0x84030201);
            Assert.AreEqual(new byte[] { 1, 2, 3, 0x84 }, output.ToArray());
        }
        [Test]
        public void WriteFixed32_ShouldHandleFloats()
        {
            var output = new MemoryStream();
            float value = (float)Math.PI;
            new MessageWriter(output).WriteFixed32(value);

            Assert.AreEqual(AsBytes(value), output.ToArray());
        }
        [Test]
        public void WriteFixed64_ShouldPutLeastSignificantBitsFirst()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteFixed64(0x0807060504030201);
            Assert.AreEqual(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }, output.ToArray());
        }
        [Test]
        public void WriteFixed64_ShouldHandleUnsigned()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteFixed64((ulong)0x08807060504030201);
            Assert.AreEqual(new byte[] { 1, 2, 3, 4, 5, 6, 7, 0x88 }, output.ToArray());
        }
        [Test]
        public void WriteFixed64_ShouldHandleDoubles()
        {
            MemoryStream output = new MemoryStream();
            new MessageWriter(output).WriteFixed64(Math.PI);

            Assert.AreEqual(AsBytes(Math.PI), output.ToArray());
        }
        [Test]
        public void WriteVarint_ShouldWriteTenBytesForMaxValue64()
        {
            MemoryStream output = new MemoryStream();
            new MessageWriter(output).WriteVarint(~0L);
            Assert.AreEqual(10, output.Length);
        }

        class MessageWithString
        {
            [Tag(1)]
            public string Value { get; set; }
        }
        [Test]
        public void WriteMessage_ShouldTreatNullStringAsEmpty()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteMessage(new MessageWithString());
            Assert.AreEqual(MessageWriter.Write(new MessageWithString(){ Value = string.Empty }), output.ToArray());
        }

        unsafe static byte[] AsBytes(float value)
        {
            return CopyBytes((byte*)&value, sizeof(float));
        }

        unsafe static byte[] AsBytes(double value)
        {
            return CopyBytes((byte*)&value, sizeof(double));
        }

        unsafe static byte[] CopyBytes(void* ptr, int count)
        {
            var bytes = new byte[count];
            for(int i = 0; i != bytes.Length; ++i)
                bytes[i] = ((byte*)ptr)[i];
            return bytes;
        }
    }
}
