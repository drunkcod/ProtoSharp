using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.IO;
using NUnit.Framework.SyntaxHelpers;

namespace ProtoSharp.Core
{
    [TestFixture]
    public class PackedEncodingTests
    {
        class PackedVarint<T>
        {
            readonly List<T> data = new List<T>();

            [Tag(4, Packed = true)]
            public List<T> Data { get { return data; } }
        }

        class PackedFixed<T>
        {
            readonly List<T> data = new List<T>();

            [Tag(4, Packed = true, UseFixed = true)]
            public List<T> Data { get { return data; } }
        }

        class Test4 : PackedVarint<int>
        {}

        static byte[] BytesFor<T>(T message)
        {
            var bytes = new MemoryStream();
            Serializer.Serialize(bytes, message);
            return bytes.ToArray();
        }

        [Test]//http://code.google.com/intl/sv-SE/apis/protocolbuffers/docs/encoding.html
        public void Test4_example_from_documentation_serialize()
        {
            var message = new Test4();
            message.Data.Add(3);
            message.Data.Add(270);
            message.Data.Add(86942);

            Assert.That(BytesFor(message), Is.EquivalentTo(new byte[]{
                0x22,         // tag (field number 4, wire type 2)
                06,         // payload size (6 bytes)
                03,         // first element (varint 3)
                0x8E, 02,     // second element (varint 270)
                0x9E, 0xA7, 05, // third element (varint 86942)
            }));
        }
        [Test]
        public void Test4_example_from_documentation_deserialize()
        {
            var bytes = new MemoryStream(new byte[] { 0x22, 6, 3, 0x8e, 2, 0x9e, 0xa7, 5});
            var message = Serializer.Deserialize<Test4>(bytes);

            Assert.That(message.Data, Is.EquivalentTo(new []{3, 270, 86942}));
        }
        [Test]
        public void Should_support_Varint64()
        {
            var message = new PackedVarint<long>();
            message.Data.Add(1);

            Assert.That(BytesFor(message), Is.EquivalentTo(new byte[] {0x22, 1, 1}));
        }
        [Test]
        public void Should_support_unsigned_Varint64()
        {
            var message = new PackedVarint<ulong>();
            message.Data.Add(1);

            Assert.That(BytesFor(message), Is.EquivalentTo(new byte[] { 0x22, 1, 1 }));
        }
        [Test]
        public void Should_support_Fixed32()
        {
            var message = new PackedFixed<int>();
            message.Data.Add(1);

            Assert.That(BytesFor(message), Is.EquivalentTo(new byte[] { 0x22, 4, 0, 0, 0, 1 }));
        }
        [Test]
        public void Should_support_Fixed64()
        {
            var message = new PackedFixed<long>();
            message.Data.Add(3);

            Assert.That(BytesFor(message), Is.EquivalentTo(new byte[] { 0x22, 8, 0, 0, 0, 0, 0, 0, 0, 3 }));
        }
        [Test]
        public void Empty_field_should_not_appear_in_encoded_message()
        {
            var message = new Test4();

            Assert.That(BytesFor(message), Is.EquivalentTo(new byte[0]));
        }
        [Test]
        public void Should_throw_for_String_types()
        {
            Expected<NotSupportedException>(() => BytesFor(new PackedVarint<string>()));
        }

        delegate void Action();
        static void Expected<T>(Action action) where T : Exception
        {
            try
            {
                action();
                Assert.Fail("Expected exception of type \"{0}\".", typeof(T).Name);
            }
            catch (T) { }
            catch (Exception e)
            {
                if (e.InnerException.GetType() != typeof(T))
                    throw;
            }
        }
    }
}
