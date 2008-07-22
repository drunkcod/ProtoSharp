using System;
using System.Collections.Generic;
using System.Diagnostics;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance.Benchmarks
{
    class RepeatedItemSerializationBenchmark : SerializationBenchmarkBase<MessageWithRepeatedItem>
    {
        public RepeatedItemSerializationBenchmark(int count, int seed, int iterations)
            : base(1, seed, iterations)
        {}

        protected override string Name { get { return "List<int> Serialization"; } }

        protected override void Serialize(IBenchmarkAdapter target, MessageWithRepeatedItem value)
        {
            target.Serialize(value);
        }

        const int Items = 2000;

        protected override MessageWithRepeatedItem Generate(Random rand)
        {
            var data = new List<int>();
            for(int i = 0; i != Items; ++i)
                data.Add(rand.Next(0, 1 << 20));
            return new MessageWithRepeatedItem(){ Value = data };
        }

    }
}
