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
        }

        public override void Serialize(MessageWithInt32 item){ _writer.WriteMessage(item); }
        public override void Serialize(MessageWithUInt32 item) { _writer.WriteMessage(item); }
        public override void Serialize(MessageWithSInt32 item) { _writer.WriteMessage(item); }

        MessageWriter _writer;
    }
}
