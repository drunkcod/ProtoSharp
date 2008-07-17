using NUnit.Framework;
using ProtoSharp.Core;

namespace ProtoSharp.Tests
{
    [TestFixture]
    public class MessageTests
    {
        class Parsable
        {
            public static Parsable Parse(string s)
            {
                return new Parsable(){ _value = s };
            }

            public string Value { get { return _value; } }

            string _value;
        }
        [Test]
        public void CreateDefaultItem_ShouldBeAbleToUseParseMethodIfAvailable()
        {
            Assert.AreEqual("Parsed", Message.CreateDefaultItem<Parsable>("Parsed").Value);
        }

        class TryParseable
        {
            public static bool TryParse(string s, out TryParseable result)
            {
                result = new TryParseable() { _value = s };
                return true;
            }

            public string Value { get { return _value; } }

            string _value;
        }
        [Test]
        public void CreateDefaultItem_ShouldBeAbleToUseTryParseMethodIfAvailable()
        {
            Assert.AreEqual("TryParsed", Message.CreateDefaultItem<TryParseable>("TryParsed").Value);
        }
        
        class StringConstructable
        {
            public StringConstructable(string s)
            {
                _value = s;
            }

            public string Value { get { return _value; } }

            string _value;
        }
        [Test]
        public void CreateDefaultItem_ShouldBeAbleToUseConstructorTakingString()
        {
            Assert.AreEqual("Constructed", Message.CreateDefaultItem<StringConstructable>("Constructed").Value);
        }
    }
}
