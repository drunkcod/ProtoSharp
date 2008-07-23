using ProtoSharp.Performance.Messages;
using System;

namespace ProtoSharp.Performance.Benchmarks
{
    class ByteArrayDeserializationBenchmark : DeserializationBenchmarkBase<MessageWithBytes>
    {
        public ByteArrayDeserializationBenchmark(int iterations) : base(iterations) { }

        protected override string Name { get { return "byte[] Deserialization"; } }

        protected override void SerializeExemplar(IBenchmarkAdapter target)
        {
            target.Serialize(new MessageWithBytes() { Value = new byte[128] });
        }

        protected override void Deserialize(IBenchmarkAdapter target, out MessageWithBytes item)
        {
            target.Deserialize(out item);
        }
    }
}
