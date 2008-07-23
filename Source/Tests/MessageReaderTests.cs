using ProtoSharp.Core;
using ProtoSharp.Tests.Messages;
using NUnit.Framework;

namespace ProtoSharp.Tests
{
    using System;
    using MessageWriter = ProtoSharp.Core.MessageWriter;

    [TestFixture]
    public class MessageReaderTests
    {
        [Test]
        public void Read_Test1SimpleMessage()
        {
            var message = MessageReader.Read<Test1>(EncodingTests.SimpleMessage);
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
        public void Read_ShouldRaiseFieldMissingForUnknownTag()
        {
            var reader = new MessageReader(EncodingTests.Test2Testing);
            bool fieldMissingRaised = false;
            reader.FieldMissing += (sender, e) => fieldMissingRaised = true;
           
            reader.Read<Test1>();

            Assert.IsTrue(fieldMissingRaised);
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
    }
}
