using System;
using System.IO;
using NUnit.Framework;

namespace ProtoSharp.Core.MessageWriterTests
{
    [TestFixture]
    public class WriteVarint
    {
        [Test]
        public void ShouldStore32BitMinusOneInTenBytes()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteVarint(-1);
            Assert.AreEqual(10, output.Length);
        }
        [Test]
        public void ShouldStore64BitMinusOneInTenBytes()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteVarint(-1L);
            Assert.AreEqual(10, output.Length);
        }
        [Test]
        public void ShouldEncodeMinusOneCorrectly()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteVarint(-1);
            Assert.AreEqual(new byte[] 
            {
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x1
            }, output.ToArray());
        }
        [Test]
        public void ShouldStoreSingleByte42()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteVarint(42);
            Assert.AreEqual(new byte[] { 42 }, output.ToArray());
        }
        [Test]
        public void ShouldStoreSingleByteFor42L()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteVarint(42L);
            Assert.AreEqual(new byte[] { 42 }, output.ToArray());
        }
        [Test]
        public void ShouldStore150InTwoBytes()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteVarint(150);
            Assert.AreEqual(new byte[] { 0x96, 0x01 }, output.ToArray());
        }
        [Test]
        public void ShouldStore0x4000InThreeBytes()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteVarint(1 << 14);
            Assert.AreEqual(new byte[] { 0x80, 0x80, 1 }, output.ToArray());
        }
        [Test]
        public void ShouldStore0x200000InFourBytes()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteVarint(1 << 21);
            Assert.AreEqual(new byte[] { 0x80, 0x80, 0x80, 1 }, output.ToArray());
        }
        [Test]
        public void ShouldStoreNineBytesForMaxInt64()
        {
            var output = new MemoryStream();
            new MessageWriter(output).WriteVarint(Int64.MaxValue);
            Assert.AreEqual(9, output.ToArray().Length);
        }
    }
}
