using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace ProtoSharp.Core
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

        class ClassWithList<T>
        {
            public ClassWithList(List<T> list)
            {
                _list = list;
            }

            public List<T> List { get { return _list; } }

            List<T> _list;
        }
        [Test]
        public void Read_ShouldCallAdd()
        {
            var target = new ClassWithList<int>(new List<int>());
            IFieldIO io;
            RepeatedFieldIO.TryCreate(target.GetType().GetProperty("List"), out io);

            io.Read(target, 42);

            Assert.AreEqual(42, target.List[0]);
        }

    }
}
