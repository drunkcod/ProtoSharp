using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace ProtoSharp.Core
{
    public class MinimalCollection<T>
    {
        public MinimalCollection(params object[] items) 
        {
            _items = items;
        }

        public IEnumerator GetEnumerator() { return _items.GetEnumerator(); }
        public void Add(T obj) { }

        object[] _items;
    }

    [TestFixture]
    public class RepeatedFileIOTests
    {
        public MinimalCollection<object> MinimalCollection { get { return new MinimalCollection<object>(); } }
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
    }
}
