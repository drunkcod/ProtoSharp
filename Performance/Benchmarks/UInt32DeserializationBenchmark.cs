using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance.Benchmarks
{
    class UInt32DeserializationBenchmark : DeserializationBenchmarkBase<MessageWithUInt32>
    {
        public UInt32DeserializationBenchmark(int iterations) : base(iterations) { }

        protected override string Name { get { return "UInt32 Deserialization"; } }

        protected override void SerializeExemplar(IBenchmarkAdapter target)
        {
            target.Serialize(new MessageWithUInt32() { Value = 42 });
        }

        protected override void Deserialize(IBenchmarkAdapter target, out MessageWithUInt32 item)
        {
            target.Deserialize(out item);
        }
    }
}
