using System;
using NUnit.Framework;

namespace ProtoSharp.Core.MessageWriterTests
{
    [TestFixture]
    public class WriteMessageTests
    {
        class MessageWithMultipleFieldsInWonkyOrder
        {
            [Tag(2)]
            public int A { get; set; }
            [Tag(1)]
            public int B { get; set; }
            [Tag(3)]
            public int C { get; set; }
        }
        [Test]
        public void ShouldWriteFieldsInOrder()
        {
            Assert.AreEqual(new byte[]
            {
                8, 0,
                16, 0,
                24, 0
            }, MessageWriter.Write(new MessageWithMultipleFieldsInWonkyOrder()));
        }
    }
}
