using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance.Benchmarks
{
    class SInt32DeserializationBenchmark : DeserializationBenchmarkBase<MessageWithSInt32>
    {
        public SInt32DeserializationBenchmark(int iterations) : base(iterations) { }

        protected override string Name { get { return "SInt32 Deserialization"; } }

        protected override void SerializeExemplar(IBenchmarkAdapter target)
        {
            target.Serialize(new MessageWithSInt32() { Value = -127 });
        }

        protected override void Deserialize(IBenchmarkAdapter target, out MessageWithSInt32 item)
        {
            target.Deserialize(out item);
        }
    }
}
