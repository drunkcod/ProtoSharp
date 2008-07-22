using ProtoSharp.Performance.Messages;
using System;

namespace ProtoSharp.Performance.Benchmarks
{
    class Fixed32DeserializationBenchmark : DeserializationBenchmarkBase<MessageWithFixed32>
    {
        public Fixed32DeserializationBenchmark(int iterations) : base(iterations) { }

        protected override string Name { get { return "Fixed32 Deserialization"; } }

        protected override void SerializeExemplar(IBenchmarkAdapter target)
        {
            target.Serialize(new MessageWithFixed32() { Unsigned = 1, Signed = -2, Float = (float)Math.PI});
        }

        protected override void Deserialize(IBenchmarkAdapter target, out MessageWithFixed32 item)
        {
            target.Deserialize(out item);
        }
    }
}
