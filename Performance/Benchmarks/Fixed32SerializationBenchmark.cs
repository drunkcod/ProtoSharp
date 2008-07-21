using System;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance.Benchmarks
{
    class Fixed32SerializationBenchmark : SerializationBenchmarkBase<MessageWithFixed32>
    {
        public Fixed32SerializationBenchmark(int count, int seed, int iterations) : base(count, seed, iterations)
        { }

        protected override string Name { get { return "Fixed Serialization"; } }

        protected override void Serialize(IBenchmarkAdapter target, MessageWithFixed32 value)
        {
            target.Serialize(value);
        }

        protected override MessageWithFixed32 Generate(Random rand)
        {
            return new MessageWithFixed32() { Integer = rand.Next(0, 1 << 20), Float = (float)rand.NextDouble() };
        }
    }
}
