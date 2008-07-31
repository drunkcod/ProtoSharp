using System;
using System.IO;
using ProtoSharp.Core;
using ProtoSharp.Tests.Messages;
using NUnit.Framework;

namespace ProtoSharp.Core.MessageReaderTests
{
    [TestFixture]
    public class MessageReaderClassTests
    {
        [Test]
        public void Read_Test1SimpleMessage()
        {
            var message = MessageReader.Read<Test1>(EncodingTests.SimpleMessage);
            Assert.AreEqual(150, message.A);
        }
        [Test]
        public void Read_Test1Nullable()
        {
            var message = MessageReader.Read<Test1Nullable>(EncodingTests.SimpleMessage);
            Assert.AreEqual(150, message.A);
        }
        [Test]
        public void Read_Test2Testing()
        {
            var message = MessageReader.Read<Test2>(EncodingTests.Test2Testing);
            Assert.AreEqual("testing", message.B);
        }
        [Test]
        public void Read_Test3EmbeddedMessage()
        {
            var message = MessageReader.Read<Test3>(EncodingTests.Test3EmbeddedMessage);
            Assert.AreEqual(150, message.C.A);
        }
        [Test]
        public void Read_WithRepeatedTest1ShouldHaveCorrectNumberOfItems()
        {
            var message = new WithRepeatedTest1();
            message.Data.Add(new Test1() { A = 42 });
            message.Data.Add(new Test1() { A = 0xbeef });

            var expected = MessageWriter.Write(message);

            Assert.AreEqual(2,
                MessageReader.Read<WithRepeatedTest1>(expected).Data.Count);
        }
        [Test]
        public void Read_ShouldSupportListOfInt()
        {
            var message = new MessageWithListOfInt();
            message.Data.Add(42 );
            message.Data.Add(0xbeef);

            var expected = MessageWriter.Write(message);

            Assert.AreEqual(2,
                MessageReader.Read<MessageWithListOfInt>(expected).Data.Count);
        }

        class Test1Extended : Test1//don't do this...
        {
            public int TheAnswer { get { return 42; } }
        }
        [Test]
        public void Read_ShouldIgnorePropertiesWithoutTag()
        {
            var message = MessageReader.Read<Test1Extended>(EncodingTests.SimpleMessage);
            Assert.AreEqual(150, message.A);
        }
        [Test]
        public void Read_ShouldHandleInt64()
        {
            var message = new Test1Ex64();
            message.A= 0x7f00000080000000;
            Assert.AreEqual(0x7f00000080000000,
                MessageReader.Read<Test1Ex64>(MessageWriter.Write(message)).A);
        }
        [Test]
        public void Read_ShouldHandleSInt32()        
        {
            var message = new WithVarint32();
            message.SInt32 = -1;
            Assert.AreEqual(-1, 
                MessageReader.Read<WithVarint32>(MessageWriter.Write(message)).SInt32);
        }
        [Test]
        public void Read_ShouldHandleSInt64()
        {
            var message = new WithVarint64();
            message.SInt64 = -1;
            Assert.AreEqual(-1,
                MessageReader.Read<WithVarint64>(MessageWriter.Write(message)).SInt64);
        }
        [Test]
        public void Read_ShouldDiscardExtraBytesForVarint32()
        {
            var message = new Test1Ex64();
            message.A = 0x7000000000000042;
            Assert.AreEqual((int)message.A,
                MessageReader.Read<Test1>(MessageWriter.Write(message)).A);
        }
        [Test]
        public void Read_ShouldHandleUInt32()
        {
            var message = new WithVarint32();
            message.UInt32 = 0x80000000;
            Assert.AreEqual(0x80000000,
                MessageReader.Read<WithVarint32>(MessageWriter.Write(message)).UInt32);
        }
        [Test]
        public void Read_ShouldHandleUInt64()
        {
            var message = new WithVarint64();
            message.UInt64 = 0x8000000000000000;
            Assert.AreEqual(0x8000000000000000,
                MessageReader.Read<WithVarint64>(MessageWriter.Write(message)).UInt64);
        }
        [Test]
        public void Read_ShouldSupportFixedInt32()
        {
            var message = new WithFixed32();
            message.Int32 = 1;
            Assert.AreEqual(1,
                MessageReader.Read<WithFixed32>(MessageWriter.Write(message)).Int32);
        }
        [Test]
        public void Read_ShouldSupportFixedUInt32()
        {
            var message = new WithFixed32();
            message.UInt32 = 0x80000001;
            Assert.AreEqual(0x80000001,
                MessageReader.Read<WithFixed32>(MessageWriter.Write(message)).UInt32);
        }
        [Test]
        public void Read_ShouldSupportFloat()
        {
            var message = new WithFixed32();
            message.Float = 3.1415f;
            Assert.AreEqual(3.1415f,
                MessageReader.Read<WithFixed32>(MessageWriter.Write(message)).Float);
        }
        [Test]
        public void Read_ShouldSupportDouble()
        {
            var message = new WithFixed64();
            message.Double= Math.PI;
            Assert.AreEqual(Math.PI,
                MessageReader.Read<WithFixed64>(MessageWriter.Write(message)).Double);
        }
        [Test]
        public void Read_ShouldSupportFixedInt64()
        {
            var message = new WithFixed64();
            message.Int64 = 0x180000001;
            Assert.AreEqual(0x180000001,
                MessageReader.Read<WithFixed64>(MessageWriter.Write(message)).Int64);
        }
        [Test]
        public void Read_ShouldSupportFixedUInt64()
        {
            var message = new WithFixed64();
            message.UInt64 = 0x800000010000001;
            Assert.AreEqual(0x800000010000001,
                MessageReader.Read<WithFixed64>(MessageWriter.Write(message)).UInt64);
        }
        [Test]
        public void Read_ShouldSupportBooleans()
        {
            var message = new WithBool(){ Boolean = true };
            Assert.AreEqual(message.Boolean,
                MessageReader.Read<WithBool>(MessageWriter.Write(message)).Boolean);
        }
        [Test]
        public void Read_ShouldSupportEnums()
        {
            var message = new WithEnum() { WireType = WireType.String};
            Assert.AreEqual(message.WireType,
                MessageReader.Read<WithEnum>(MessageWriter.Write(message)).WireType);
        }
        [Test]
        public void Read_ShouldSupportBytes()
        {
            var message = new WithBytes() { Bytes = new byte[]{1, 2, 3} };
            Assert.AreEqual(message.Bytes,
                MessageReader.Read<WithBytes>(MessageWriter.Write(message)).Bytes);
        }
        [Test]
        public void Read_ShouldHandleDateTime()
        {
            var message = MessageReader.Read<MessageWithDateTime>(MessageWriter.Write(new Test1()));
            Assert.AreEqual(UnixTime.Epoch, message.TimeStamp);
        }
        [Test]
        public void Read_ShouldHandleDecimal()
        {
            Assert.AreEqual(3.1415, MessageReader.Read<MessageWithDecimal>(
                MessageWriter.Write(new MessageWithDecimal() { Value = (decimal)3.1415 })).Value);
        }
        [Test]
        public void Read_ShouldHandleInt16()
        {
            var message = MessageReader.Read<MessageWithInt16>(new byte[]
            {
                1 << 3 | (int)WireType.Varint, 1,
                2 << 3 | (int)WireType.Varint, 1,
                3 << 3 | (int)WireType.Varint, 42
            });
            Assert.AreEqual(message.Default, 1);
            Assert.AreEqual(message.ZigZag, -1);
            Assert.AreEqual(message.Unsigned, 42);
        }
        [Test]
        public void Read_ShouldHandleGroups()
        {
            Assert.AreEqual(150, MessageReader.Read<Test3>(new byte[] 
            {
                3 << 3 | (int)WireType.StartGroup,
                0x08, 0x96, 0x01, //Submessage
                3 << 3 | (int)WireType.EndGroup
            }).C.A);
        }
        [Test]
        public void Read_ShouldRaiseMissingFieldsContainingAllMissingFields()
        {
            var input = new MemoryStream();
            new MessageWriter(input)
                .WriteHeader(2, WireType.Varint).WriteVarint(42)
                .WriteHeader(3, WireType.String).WriteString("Hello World!");
            input.Position = 0;
            var reader = new MessageReader(input);
            UnknownFieldCollection unknown = null;
            reader.MissingFields += (sender, e) => unknown = e.Fields;

            reader.Read<Test1>();

            Assert.AreEqual(2, unknown.Count);
        }
        [Test,ExpectedException(typeof(NotSupportedException))]
        public void Read_ShouldThrowNotSupportedForInvalidWireType()
        {
            new MessageReader(0xF).Read<Test1>();
        }
    }
}
