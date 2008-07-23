using ProtoSharp.Performance.Messages;
using System;

namespace ProtoSharp.Performance.Benchmarks
{
    class Fixed64DeserializationBenchmark : DeserializationBenchmarkBase<MessageWithFixed64>
    {
        public Fixed64DeserializationBenchmark(int iterations) : base(iterations) { }

        protected override string Name { get { return "Fixed64 Deserialization"; } }

        protected override void SerializeExemplar(IBenchmarkAdapter target)
        {
            target.Serialize(new MessageWithFixed64() { Unsigned = 1, Signed = -2, Double = Math.PI });
        }

        protected override void Deserialize(IBenchmarkAdapter target, out MessageWithFixed64 item)
        {
            target.Deserialize(out item);
        }
    }
}
