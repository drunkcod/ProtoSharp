using System;
using System.Collections.Generic;
using System.Diagnostics;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance.Benchmarks
{
    class Int32SerializationBenchmark : SerializationBenchmarkBase<MessageWithInt32>
    {
        public Int32SerializationBenchmark(int count, int seed, int iterations): base(count, seed, iterations)
        {}

        protected override string Name { get { return "Int32 Serialization"; } }

        protected override void Serialize(IBenchmarkAdapter target, MessageWithInt32 value)
        {
            target.Serialize(value);
        }

        protected override MessageWithInt32 Generate(Random rand)
        {
            return new MessageWithInt32() { Value = rand.Next(0, 1 << 20) };
        }
    }
}
