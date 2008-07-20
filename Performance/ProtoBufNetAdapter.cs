using ProtoBuf;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance
{
    class ProtoBufNetAdapter : BenchmarkAdapterBase
    {
        public ProtoBufNetAdapter(byte[] memory)
            : base(memory)
        { }

        public override void Serialize(MessageWithInt32 item)
        {
            Serializer.Serialize(item, Memory);
        }
    }
}
