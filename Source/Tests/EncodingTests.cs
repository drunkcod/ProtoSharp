using System;
using NUnit.Framework;
using ProtoSharp.Core;

namespace ProtoSharp.Tests
{
    /*
     * Theese are taken from the documentation page at:
     * http://code.google.com/apis/protocolbuffers/docs/encoding.html
     */ 
    [TestFixture]
    public class EncodingTests
    {
        /*
         * message Test1 {
         *    required int32 a = 1;
         * }
         */
        //Test1 with a = 150
        public static readonly byte[] SimpleMessage = { 0x08, 0x96, 0x01 };
        [Test]
        public void SimpleMessageWireTypeIsVarint()
        {
            MessageReader reader = new MessageReader(SimpleMessage);
            Assert.AreEqual(WireType.Varint, reader.ReadMessageTag().WireType);
        }
        [Test]
        public void SimpleMessageTagIsOne()        
        {
            MessageReader reader = new MessageReader(SimpleMessage);
            Assert.AreEqual(1, new MessageTag(reader.ReadVarint32()).Number);
        }
        [Test]
        public void SimpleMessageValueIs150()
        {
            MessageReader reader = new MessageReader(SimpleMessage);
            reader.ReadMessageTag();
            Assert.AreEqual(150, reader.ReadVarint32());
        }

        /*
         * message Test2 {
         *   required string b = 2;
         * }
         */
        public static readonly byte[] Test2Testing = { 0x12, 0x07, 0x74, 0x65, 0x73, 0x74, 0x69, 0x6e, 0x67 };
        [Test]
        public void Test2TestingWireTypeIsString()
        {
            MessageReader reader = new MessageReader(Test2Testing);
            Assert.AreEqual(WireType.String, reader.ReadMessageTag().WireType);
        }
        [Test]
        public void Test2TestingTagIsTwo()
        {
            MessageReader reader= new MessageReader(Test2Testing);
            Assert.AreEqual(2, reader.ReadMessageTag().Number);
        }
        [Test]
        public void Test2TestingValueIs_testing()
        {
            MessageReader reader = new MessageReader(Test2Testing);
            reader.ReadMessageTag();
            reader.ReadVarint32();
            Assert.AreEqual("testing", reader.ReadString());
        }
        /*
         * message Test3 {
         *   required Test1 c = 3 
         * }
         */
        public static readonly byte[] Test3EmbeddedMessage = { 0x1a, 0x03, 0x08, 0x96, 0x01 };
        public void EmbeddedMessage_ShouldHaveWireTypeString()
        {
            MessageReader reader = new MessageReader(Test3EmbeddedMessage);
            Assert.AreEqual(WireType.String, reader.ReadMessageTag().WireType);
        }
        public void Test3EmbeddedMessageHasTagThree()
        {
            MessageReader reader = new MessageReader(Test3EmbeddedMessage);
            Assert.AreEqual(3, reader.ReadMessageTag().Number);
        }
    }
}
