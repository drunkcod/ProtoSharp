using System;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance.Benchmarks
{
    class Fixed64SerializationBenchmark : SerializationBenchmarkBase<MessageWithFixed64>
    {
        public Fixed64SerializationBenchmark(int count, int seed, int iterations)
            : base(count, seed, iterations)
        { }

        protected override string Name { get { return "Fixed64 Serialization"; } }

        protected override void Serialize(IBenchmarkAdapter target, MessageWithFixed64 value)
        {
            target.Serialize(value);
        }

        protected override MessageWithFixed64 Generate(Random rand)
        {
            return new MessageWithFixed64() { Signed = rand.Next(0, 1 << 20), Double= rand.NextDouble() };
        }
    }
}
