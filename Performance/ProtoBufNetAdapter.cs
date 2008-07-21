using ProtoBuf;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance
{
    class ProtoBufNetAdapter : BenchmarkAdapterBase
    {
        public ProtoBufNetAdapter(byte[] memory) : base(memory) { }

        public override void Serialize(MessageWithInt32 item){ Serializer.Serialize(Memory, item); }
        public override void Serialize(MessageWithUInt32 item) { Serializer.Serialize(Memory, item); }
        public override void Serialize(MessageWithSInt32 item) { Serializer.Serialize(Memory, item); }
        public override void Serialize(MessageWithFixed32 item) { Serializer.Serialize(Memory, item); }
        public override void Serialize(MessageWithFixed64 item) { Serializer.Serialize(Memory, item); }
        public override void Serialize(MessageWithString item) { Serializer.Serialize(Memory, item); }
        public override void Serialize(MessageWithBytes item) { Serializer.Serialize(Memory, item); }
    }
}
