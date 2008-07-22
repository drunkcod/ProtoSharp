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
        public override void Serialize(MessageWithRepeatedItem item) { Serializer.Serialize(Memory, item); }
        public override void Serialize(Person item) { Serializer.Serialize(Memory, item); }

        public override void Deserialize(out MessageWithInt32 item) { item = Serializer.Deserialize<MessageWithInt32>(Memory); }
        public override void Deserialize(out MessageWithUInt32 item) { item = Serializer.Deserialize<MessageWithUInt32>(Memory); }
        public override void Deserialize(out MessageWithSInt32 item) { item = Serializer.Deserialize<MessageWithSInt32>(Memory); }
        public override void Deserialize(out MessageWithFixed32 item) { item = Serializer.Deserialize<MessageWithFixed32>(Memory); }
    }
}
