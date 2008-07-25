using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using ProtoSharp.Core;
using System;
using ProtoSharp.Tests.Messages;

namespace ProtoSharp.Tests
{
    [TestFixture]
    public class NullableFieldIOTests
    {
        class MessageWithNullableInt
        {
            public int? Value { get; set; }
        }

        [Test]
        public void FieldType_ShouldBeBaseType()
        {
            var io = new NullableFieldIO(typeof(MessageWithNullableInt).GetProperty("Value"));
            Assert.AreEqual(typeof(int), io.FieldType);
        }
        [Test,ExpectedException(typeof(NotSupportedException))]
        public void ShouldThrowNotSupportedExceptionIfPropertyNotNullable()
        {
            new NullableFieldIO(typeof(Test1).GetProperty("A"));
        }
    }
}
