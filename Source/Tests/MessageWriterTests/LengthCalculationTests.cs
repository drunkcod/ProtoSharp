using System.IO;
using NUnit.Framework;

namespace ProtoSharp.Core.MessageWriterTests
{
    [TestFixture]
    public class LengthCalculationTests
    {
        [Test]
        public void ShouldReturnOneAsLengthOf0x7f()
        {
            Assert.AreEqual(1, new MessageWriter(Stream.Null).LengthVarint(0x7f));
        }
        [Test]
        public void ShouldReturnTenAsLengthOfMinusOne()
        {
            Assert.AreEqual(10, new MessageWriter(Stream.Null).LengthVarint(-1));
        }   
    }
}
