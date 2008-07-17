using System.Reflection;

namespace ProtoSharp.Core
{
    class MessageFieldFloat : MessageField
    {
        public MessageFieldFloat(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Fixed32) { }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadFloat();
        }
        protected override void DoWrite(object value, MessageWriter writer)
        {            
            writer.WriteFixed32((float)value);
        }
    }
}
