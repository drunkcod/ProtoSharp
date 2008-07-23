using ProtoSharp.Core;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance
{
    class MessageWriterAdapter : BenchmarkAdapterBase
    {
        public MessageWriterAdapter(byte[] memory)
            : base(memory)
        {
            _writer = new MessageWriter(Memory);
            _reader = new MessageReader(new ByteReader(Memory));
        }

        public override void Serialize(MessageWithInt32 item){ _writer.WriteMessage(item); }
        public override void Serialize(MessageWithUInt32 item) { _writer.WriteMessage(item); }
        public override void Serialize(MessageWithSInt32 item) { _writer.WriteMessage(item); }
        public override void Serialize(MessageWithFixed32 item) { _writer.WriteMessage(item); }
        public override void Serialize(MessageWithFixed64 item) { _writer.WriteMessage(item); }
        public override void Serialize(MessageWithString item) { _writer.WriteMessage(item); }
        public override void Serialize(MessageWithBytes item) { _writer.WriteMessage(item); }
        public override void Serialize(MessageWithRepeatedItem item) { _writer.WriteMessage(item); }
        public override void Serialize(Person item) { _writer.WriteMessage(item); }

        public override void Deserialize(out MessageWithInt32 item) { item = _reader.Read<MessageWithInt32>(); }
        public override void Deserialize(out MessageWithUInt32 item) { item = _reader.Read<MessageWithUInt32>(); }
        public override void Deserialize(out MessageWithSInt32 item) { item = _reader.Read<MessageWithSInt32>(); }
        public override void Deserialize(out MessageWithFixed32 item) { item = _reader.Read<MessageWithFixed32>(); }
        public override void Deserialize(out MessageWithFixed64 item) { item = _reader.Read<MessageWithFixed64>(); }
        public override void Deserialize(out MessageWithString item) { item = _reader.Read<MessageWithString>(); }

        MessageWriter _writer;
        MessageReader _reader;
    }
}
