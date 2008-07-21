using System;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance.Benchmarks
{
    class SInt32SerializationBenchmark : SerializationBenchmarkBase<MessageWithSInt32>
    {
        public SInt32SerializationBenchmark(int count, int seed, int iterations)
            : base(count, seed, iterations)
        { }

        protected override string Name { get { return "SInt32 Serialization"; } }

        protected override void Serialize(IBenchmarkAdapter target, MessageWithSInt32 value)
        {
            target.Serialize(value);
        }

        protected override MessageWithSInt32 Generate(Random rand)
        {
            return new MessageWithSInt32() { Value = rand.Next(-(1 << 19), 1 << 19) };
        }
    }
}
