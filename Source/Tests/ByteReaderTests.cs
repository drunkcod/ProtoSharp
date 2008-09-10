using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace ProtoSharp.Core
{
    [TestFixture]
    public class ByteReaderTests
    {
        [Test]
        public void ShouldBeCreatableFromStream()
        {
            new ByteReader(new MemoryStream() as Stream);
        }
        [Test]
        public void GetByte_ShouldAdvanceStream()
        {
            var input = new MemoryStream(new byte[] { 42 });
            new ByteReader(input).GetByte();
            Assert.AreEqual(1, input.Position);
        }
        [Test]
        public void GetByte_ShouldGetNextByte()
        {
            var input = new MemoryStream(new byte[] { 42 });
            Assert.AreEqual(42, new ByteReader(input).GetByte());
        }
        [Test]
        public void GetBytes_ShouldReturnSegmentWithSourceBytes()
        {
            var input = new MemoryStream(new byte[] { 1, 2, 3 });
            var reader = new ByteReader(input);
            reader.GetByte();

            Assert.That(new byte[] { 2, 3 }, Is.SubsetOf(reader.GetBytes(2).Array));
        }
        [Test]
        public void GetByteReader_ShouldAdvanceStream()
        {
            var input = new MemoryStream(new byte[] { 1, 2, 3 });
            new ByteReader(input).GetByteReader(2);

            Assert.AreEqual(2, input.Position);
        }
        [Test]
        public void GetByteReader_ShouldReturnReaderThatStartAtCurrentPosition()
        {
            var input = new MemoryStream(new byte[] { 1, 2, 3 });

            Assert.AreEqual(1,             
                new ByteReader(input).GetByteReader(2).GetByte());
        }
        [Test]
        public void EndOfStream_ShouldBeTrueIfAllBytesConsumed()
        {
            var input = new MemoryStream(new byte[] { 1, 2, 3 });
            var reader = new ByteReader(input);
            reader.GetByteReader((int)input.Length);

            Assert.IsTrue(reader.EndOfStream);
        }
        [Test]
        public void GetFloat_ShouldBeCompatibleWithWriteFloat()
        {
            var input = new MemoryStream();
            new MessageWriter(input).WriteFixed((float)Math.PI);
            input.Position = 0;

            Assert.AreEqual((float)Math.PI, new ByteReader(input).GetFloat());
        }
    }
}
