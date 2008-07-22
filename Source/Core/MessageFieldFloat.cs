using System.Reflection;

namespace ProtoSharp.Core
{
    class MessageFieldFloat : MessageFieldFixed<float>
    {
        public MessageFieldFloat(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Fixed32) { }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadFloat();
        }
    }
}
