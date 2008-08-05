using System;
using System.IO;
using NUnit.Framework;
using ProtoSharp.Core.MessageFields;
using ProtoSharp.Tests.Messages;

namespace ProtoSharp.Core
{
    [TestFixture]
    public class MessageWriterClassTests
    {
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
        public void WriteMessage_ShouldSupportNullableHavingValue()
        {
            var output = new MemoryStream();
            var data = new Test1Nullable();
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
        public void WriteMessage_ShouldHandleListOfInt()
        {
            var message = new MessageWithListOfInt();
            message.Data.AddRange(new int[] { 1, 2, 3 });

            Assert.AreEqual(new byte[] { 8, 1, 8, 2, 8, 3}, MessageWriter.Write(message));
        }
        [Test]
        public void WriteFixed32_ShouldPutLeastSignificantBitsFirst()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteFixed(0x04030201);
            Assert.AreEqual(new byte[] { 1, 2, 3, 4 }, output.ToArray());
        }
        [Test]
        public void WriteFixed32_ShouldHandleUnsigned()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteFixed((uint)0x84030201);
            Assert.AreEqual(new byte[] { 1, 2, 3, 0x84 }, output.ToArray());
        }
        [Test]
        public void WriteFixed32_ShouldHandleFloats()
        {
            var output = new MemoryStream();
            float value = (float)Math.PI;
            new MessageWriter(output).WriteFixed(value);

            Assert.AreEqual(AsBytes(value), output.ToArray());
        }
        [Test]
        public void WriteFixed64_ShouldPutLeastSignificantBitsFirst()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteFixed(0x0807060504030201);
            Assert.AreEqual(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }, output.ToArray());
        }
        [Test]
        public void WriteFixed64_ShouldHandleUnsigned()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteFixed((ulong)0x08807060504030201);
            Assert.AreEqual(new byte[] { 1, 2, 3, 4, 5, 6, 7, 0x88 }, output.ToArray());
        }
        [Test]
        public void WriteFixed64_ShouldHandleDoubles()
        {
            MemoryStream output = new MemoryStream();
            new MessageWriter(output).WriteFixed(Math.PI);

            Assert.AreEqual(AsBytes(Math.PI), output.ToArray());
        }
        [Test]
        public void WriteVarint_ShouldWriteTenBytesForMaxValue64()
        {
            MemoryStream output = new MemoryStream();
            new MessageWriter(output).WriteVarint(~0L);
            Assert.AreEqual(10, output.Length);
        }

        [Test]
        public void WriteMessage_ShouldHandleDateTime()
        {
            var output = new MemoryStream();
            var timeStamp = new DateTime(2008, 07, 25);

            new MessageWriter(output)
                .WriteHeader(1, WireType.Varint)
                .WriteZigZag(UnixTime.From(timeStamp));
            Assert.AreEqual(output.ToArray(),
            MessageWriter.Write(new MessageWithDateTime() { TimeStamp = timeStamp }));
        }
        [Test]
        public void WriteMessage_ShouldHandleDecimal()
        {
            Assert.AreEqual(MessageWriter.Write(new Test1Ex64(){ A = (long)(Math.PI * MessageFieldDecimal.Factor) }),
                MessageWriter.Write(new MessageWithDecimal(){ Value = (Decimal)Math.PI })); 
        }
        [Test]
        public void WriteMessage_ShouldHandleInt16()
        {
            Assert.AreEqual(new byte[]
            {
                1 << 3 | (int)WireType.Varint, 1,
                2 << 3 | (int)WireType.Varint, 1,
                3 << 3 | (int)WireType.Varint, 42
            }, MessageWriter.Write(new MessageWithInt16()
            {
                Default = 1, 
                ZigZag = -1,
                Unsigned = 42
            }));
        }
        [Test]
        public void WriteMessage_ShouldEncodeFixed64WithCorrectWireType()
        {
            Assert.AreEqual(new byte[]
            {
                1 << 3 | (int)WireType.Fixed64, 0, 0, 0 ,0 ,0 ,0 ,0 ,0,
                2 << 3 | (int)WireType.Fixed64, 0, 0, 0 ,0 ,0 ,0 ,0 ,0,
                3 << 3 | (int)WireType.Fixed64, 0, 0, 0 ,0 ,0 ,0 ,0 ,0
            }, MessageWriter.Write(new WithFixed64() { Int64 = 0, UInt64 = 0, Double = 0 }));
        }
        [Test]
        public void WriteMessage_ShouldSkipNullableFieldsWithoutValue()
        {
            Assert.AreEqual(new byte[0], MessageWriter.Write(new Test1Nullable()));
        }

        class MessageWithEmbeddedNullable
        {
            [Tag(1)]
            public Test1Nullable Value { get { return _value; } set { _value = value; } }

            Test1Nullable _value = new Test1Nullable();
        }
        [Test]
        public void WriteMessage_ShouldHandleEmbeddedEmptyMessages()
        {
            Assert.AreEqual(new byte[] { 1 << 3 | (byte)WireType.String, 0 }, MessageWriter.Write(new MessageWithEmbeddedNullable()));
        }
        [Test]
        public void WriteDateTime_ShouldHandle_1996_07_04()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteDateTime(new DateTime(1996, 7, 4));
            Assert.AreEqual(new byte[] 
            {
                0x80, 0xf0, 0xe8, 0xf9, 0xd7, 0x30
            }, output.ToArray());              
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
