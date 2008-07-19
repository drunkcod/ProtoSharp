using System.Collections;
using NUnit.Framework;
using ProtoSharp.Core;
using System.Collections.Generic;

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
        public List<int> ListOfInt { get { return new List<int>(); } }

        [Test]
        public void TryCreate_ShouldBeAbleToCreateForClassWithAddAndGetEnumerator()
        {
            IFieldIO io;
            Assert.IsTrue(RepeatedFieldIO.TryCreate(GetType().GetProperty("MinimalCollection"), out io));
            Assert.IsInstanceOfType(typeof(RepeatedFieldIO), io);
        }
        [Test]
        public void TryCreate_ShouldBeAbleToCreateForGenericList()
        {
            IFieldIO io;
            Assert.IsTrue(RepeatedFieldIO.TryCreate(GetType().GetProperty("ListOfInt"), out io));
            Assert.IsInstanceOfType(typeof(RepeatedFieldIO), io);
        }

    }
}
