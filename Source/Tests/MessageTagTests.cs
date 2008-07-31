using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace ProtoSharp.Core
{
    [TestFixture]
    public class MessageTagTests
    {
        [Test]
        public void ToString_ShouldContainNumberandWireType()
        {
            var actual = new MessageTag(3, WireType.String).ToString();

            Assert.IsTrue(actual.Contains("3") && actual.Contains("String"));
        }
    }
}
