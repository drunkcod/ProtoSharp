using ProtoSharp.Core.UnknownFields;

namespace ProtoSharp.Core
{
    public abstract class UnknownField
    {
        public static UnknownField Create(MessageTag tag, MessageReader reader)
        {
            switch(tag.WireType)
            {
                case WireType.Fixed64: return new UnknownFieldFixed64(tag, reader);
                case WireType.String: return new UnknownFieldString(tag, reader);
                case WireType.StartGroup: return new UnknownFieldGroup(tag, reader);
                case WireType.Fixed32: return new UnknownFieldFixed32(tag, reader);
            }
            return new UnknownFieldVarint(tag, reader.ReadInt64());
        }

        public void Serialize(MessageWriter writer)
        {
            SerializeCore(writer.WriteVarint(_tag.Value));
        }
      
        public int Number { get { return _tag.Number; } }
        public WireType WireType { get { return _tag.WireType; } }
        public object Value { get { return _value; } }

        protected UnknownField(MessageTag tag, object value)
        {
            _tag = tag;
            _value = value;
        }

        protected abstract void SerializeCore(MessageWriter writer);

        MessageTag _tag;
        object _value;
    }
}
