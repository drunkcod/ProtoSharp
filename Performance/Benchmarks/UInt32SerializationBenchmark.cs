using System;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance.Benchmarks
{
    class UInt32SerializationBenchmark : SerializationBenchmarkBase<MessageWithUInt32>
    {
        public UInt32SerializationBenchmark(int count, int seed, int iterations): base(count, seed, iterations)
        {}

        protected override string Name { get { return "UInt32 Serialization"; } }

        protected override void Serialize(IBenchmarkAdapter target, MessageWithUInt32 value)
        {
            target.Serialize(value);
        }

        protected override MessageWithUInt32 Generate(Random rand)
        {
            return new MessageWithUInt32() { Value = (uint)rand.Next(0, 1 << 20) };
        }
    }
}
