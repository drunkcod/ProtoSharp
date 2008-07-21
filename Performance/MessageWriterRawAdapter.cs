using ProtoSharp.Core;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance
{
    class MessageWriterRawAdapter : BenchmarkAdapterBase
    {
        public MessageWriterRawAdapter(byte[] memory)
            : base(memory)
        {
            _writer = new MessageWriter(Memory);
        }

        public override void Serialize(MessageWithInt32 item){ _writer.WriteVarint(item.Value); }
        public override void Serialize(MessageWithUInt32 item) { _writer.WriteVarint(item.Value); }
        public override void Serialize(MessageWithSInt32 item) { _writer.WriteZigZag(item.Value); }
        public override void Serialize(MessageWithFixed32 item) { _writer.WriteFixed(item.Signed); _writer.WriteFixed(item.Unsigned); _writer.WriteFixed(item.Float); }
        public override void Serialize(MessageWithFixed64 item) { _writer.WriteFixed(item.Signed); _writer.WriteFixed(item.Unsigned); _writer.WriteFixed(item.Double); }

        MessageWriter _writer;
    }
}
