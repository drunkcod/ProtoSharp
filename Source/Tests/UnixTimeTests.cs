using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ProtoSharp.Core;

namespace ProtoSharp.Core
{
    [TestFixture]
    public class UnixTimeTests
    {
        [Test]
        public void ShouldBegin1970_01_01()
        {
            Assert.AreEqual(new DateTime(1970, 1, 1), UnixTime.Epoch);
        }
        [Test]
        public void From_ShouldBeZeroForStartOfEpoch()
        {
            Assert.AreEqual(0, UnixTime.From(UnixTime.Epoch));
        }
        [Test]
        public void From_ShouldBeSixtyThoushandOneMinuteAfterEpoch()
        {
            Assert.AreEqual(60000, UnixTime.From(UnixTime.Epoch.AddMinutes(1)));
        }
        [Test]
        public void ToDateTime_ShouldReturnEpochForZero()
        {
            Assert.AreEqual(UnixTime.Epoch, UnixTime.ToDateTime(0));
        }
    }
}
