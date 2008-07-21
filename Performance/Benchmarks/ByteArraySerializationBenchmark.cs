using System;
using System.Collections.Generic;
using System.Diagnostics;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance.Benchmarks
{
    class ByteArraySerializationBenchmark : SerializationBenchmarkBase<MessageWithBytes>
    {
        public ByteArraySerializationBenchmark(int count, int seed, int iterations)
            : base(count, seed, iterations)
        { }

        protected override string Name { get { return "byte[] Serialization"; } }

        protected override void Serialize(IBenchmarkAdapter target, MessageWithBytes value)
        {
            target.Serialize(value);
        }

        protected override MessageWithBytes Generate(Random rand)
        {
            return new MessageWithBytes() { Value = new byte[rand.Next(0, 1 << 6)] };
        }
    }
}
