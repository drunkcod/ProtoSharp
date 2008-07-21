using System.Reflection;

namespace ProtoSharp.Core
{
    class MessageFieldDouble : MessageFieldFixed<double>
    {
        public MessageFieldDouble(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Fixed32) { }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadDouble();
        }
        protected override void DoWrite(object value, MessageWriter writer)
        {
            writer.WriteFixed((double)value);
        }
    }
}
