using NUnit.Framework;
using ProtoSharp.Tests.Messages;

namespace ProtoSharp.Core.MessageReaderTests
{
    [TestFixture]
    public class UnknownFieldTests
    {
        [Test]
        public void ShouldRaiseFieldMissingForUnknownTag()
        {
            var reader = new MessageReader(EncodingTests.Test2Testing);
            bool fieldMissingRaised = false;
            reader.MissingFields += (sender, e) => fieldMissingRaised = true;

            reader.Read<Test1>();

            Assert.IsTrue(fieldMissingRaised);
        }

        [Test]
        public void ShouldAddMissingFieldForUnkownEnumValue()
        {
            var missing = new UnknownFieldCollection();
            new MessageReader(8, (byte)WireType.MaxValid + 1).Read<WithEnum>(missing);

            Assert.AreEqual(1, missing.Count);
        }
    }
}
