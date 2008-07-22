using System.IO;
using ProtoSharp.Performance.Messages;
using System;

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
        public override void Serialize(MessageWithString item) { _writer.Write(item.Value); }
        public override void Serialize(MessageWithBytes item) { _writer.Write(item.Value); }
        public override void Serialize(MessageWithRepeatedItem item) { item.Value.ForEach(_writer.Write); }
        public override void Serialize(Person item) { throw new NotSupportedException(); }

        BinaryWriter _writer;
    }
}
