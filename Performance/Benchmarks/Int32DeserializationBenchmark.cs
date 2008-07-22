using System;
using System.Diagnostics;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance.Benchmarks
{
    class Int32DeserializationBenchmark : DeserializationBenchmarkBase<MessageWithInt32>
    {
        public Int32DeserializationBenchmark(int iterations): base(iterations) {}

        protected override string Name { get { return "Int32 Deserialization"; } }

        protected override void SerializeExemplar(IBenchmarkAdapter target)
        {
            target.Serialize(new MessageWithInt32() { Value = 42 });
        }

        protected override void Deserialize(IBenchmarkAdapter target, out MessageWithInt32 item)
        {
            target.Deserialize(out item);
        }
    }
}
