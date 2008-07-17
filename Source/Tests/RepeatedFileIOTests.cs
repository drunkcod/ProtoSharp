using System.Collections;
using NUnit.Framework;
using ProtoSharp.Core;

namespace ProtoSharp.Tests
{
    public class MinimalCollection
    {
        public IEnumerator GetEnumerator() { yield break; }
        public void Add(object obj) { }

    }

    [TestFixture]
    public class RepeatedFileIOTests
    {
        public MinimalCollection MinimalCollection { get { return new MinimalCollection(); } }

        [Test]
        public void TryCreate_ShouldBeAbleToCreateForClassWithAddAndGetEnumerator()
        {
            IFieldIO io;
            Assert.IsTrue(RepeatedFieldIO.TryCreate(GetType().GetProperty("MinimalCollection"), out io));
            Assert.IsInstanceOfType(typeof(RepeatedFieldIO), io);
        }
    }
}
