using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace ProtoSharp.Core.MessageReaderTests
{
    [TestFixture]
    public class ReadMessageTests
    {
        class SimpleMessage
        {
            [Tag(1)]
            public int A { get; set; }
            [Tag(2)]
            public int B { get; set; }
            [Tag(3)]
            public int C { get; set; }
        }
        [Test]
        public void ShouldHandleFieldsInMixedOrder()
        {
            var input = new MemoryStream();
            new MessageWriter(input)
                .WriteHeader(3, WireType.Varint).WriteVarint(3)
                .WriteHeader(1, WireType.Varint).WriteVarint(1)
                .WriteHeader(2, WireType.Varint).WriteVarint(2);
            input.Position = 0;

            var message = MessageReader.Read<SimpleMessage>(input);
            Assert.AreEqual(new int[]
            {
                1, 2, 3
            }, new int[]
            {
                message.A,
                message.B,
                message.C
            });
        }
    }
}
