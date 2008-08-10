using ProtoSharp.Core;
using ProtoSharp.Performance.Messages;
using System.IO;

namespace ProtoSharp.Performance
{
    class MessageWriterAdapter : BenchmarkAdapterBase
    {
        public MessageWriterAdapter(byte[] memory)
            : base(memory)
        {
            _writer = new MessageWriter(Stream.Null);
            _reader = new MessageReader(new ByteReader(Memory));
        }

        public override void Serialize(MessageWithInt32 item) { Serializer.Serialize(_writer, item); }
        public override void Serialize(MessageWithUInt32 item) { Serializer.Serialize(_writer, item); }
        public override void Serialize(MessageWithSInt32 item) { Serializer.Serialize(_writer, item); }
        public override void Serialize(MessageWithFixed32 item) { Serializer.Serialize(_writer, item); }
        public override void Serialize(MessageWithFixed64 item) { Serializer.Serialize(_writer, item); }
        public override void Serialize(MessageWithString item) { Serializer.Serialize(_writer, item); }
        public override void Serialize(MessageWithBytes item) { Serializer.Serialize(_writer, item); }
        public override void Serialize(MessageWithRepeatedItem item) { Serializer.Serialize(_writer, item); }
        public override void Serialize(Person item) { Serializer.Serialize(_writer, item); }

        public override void Deserialize(out MessageWithInt32 item) { item = Serializer.Deserialize<MessageWithInt32>(_reader); }
        public override void Deserialize(out MessageWithUInt32 item) { item = Serializer.Deserialize<MessageWithUInt32>(_reader); }
        public override void Deserialize(out MessageWithSInt32 item) { item = Serializer.Deserialize<MessageWithSInt32>(_reader); }
        public override void Deserialize(out MessageWithFixed32 item) { item = Serializer.Deserialize<MessageWithFixed32>(_reader); }
        public override void Deserialize(out MessageWithFixed64 item) { item = Serializer.Deserialize<MessageWithFixed64>(_reader); }
        public override void Deserialize(out MessageWithString item) { item = Serializer.Deserialize<MessageWithString>(_reader); }
        public override void Deserialize(out MessageWithBytes item) { item = Serializer.Deserialize<MessageWithBytes>(_reader); }
        public override void Deserialize(out MessageWithRepeatedItem item) { item = Serializer.Deserialize<MessageWithRepeatedItem>(_reader); }
        public override void Deserialize(out Person item) { item = Serializer.Deserialize<Person>(_reader); }

        MessageWriter _writer;
        MessageReader _reader;
    }
}
