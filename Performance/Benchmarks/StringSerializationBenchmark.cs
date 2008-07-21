using System;
using System.Collections.Generic;
using System.Diagnostics;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance.Benchmarks
{
    class StringSerializationBenchmark : SerializationBenchmarkBase<MessageWithString>
    {
        public StringSerializationBenchmark(int count, int seed, int iterations)
            : base(count, seed, iterations)
        { }

        protected override string Name { get { return "String Serialization"; } }

        protected override void Serialize(IBenchmarkAdapter target, MessageWithString value)
        {
            target.Serialize(value);
        }

        protected override MessageWithString Generate(Random rand)
        {
            return new MessageWithString() { Value = new string('*', rand.Next(0, 1 << 6)) };
        }
    }
}
