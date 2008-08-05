using NUnit.Framework;
using ProtoSharp.Tests.Messages;

namespace ProtoSharp.Core.MessageReaderTests
{
    [TestFixture]
    public class UnknownFieldTests
    {
        [Test]
        public void ShouldAddMissingFieldForUnkownEnumValue()
        {
            var missing = new UnknownFieldCollection();
            Serializer.Deserialize<WithEnum>(new MessageReader(8, (byte)WireType.MaxValid + 1), missing);

            Assert.AreEqual(1, missing.Count);
        }
    }
}
