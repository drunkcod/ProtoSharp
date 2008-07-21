using System.IO;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance
{
    class BinaryWriterAdapter : BenchmarkAdapterBase
    {
        public BinaryWriterAdapter(byte[] memory)
            : base(memory)
        {
            _writer = new BinaryWriter(Memory);
        }

        public override void Serialize(MessageWithInt32 item){ _writer.Write(item.Value); }
        public override void Serialize(MessageWithUInt32 item) { _writer.Write(item.Value); }
        public override void Serialize(MessageWithSInt32 item) { _writer.Write(item.Value); }
        public override void Serialize(MessageWithFixed32 item) { _writer.Write(item.Signed); _writer.Write(item.Unsigned); _writer.Write(item.Float); }
        public override void Serialize(MessageWithFixed64 item) { _writer.Write(item.Signed); _writer.Write(item.Unsigned); _writer.Write(item.Double); }

        BinaryWriter _writer;
    }
}
