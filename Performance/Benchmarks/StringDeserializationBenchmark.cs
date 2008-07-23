using ProtoSharp.Performance.Messages;
using System;

namespace ProtoSharp.Performance.Benchmarks
{
    class StringDeserializationBenchmark : DeserializationBenchmarkBase<MessageWithString>
    {
        public StringDeserializationBenchmark(int iterations) : base(iterations) { }

        protected override string Name { get { return "String Deserialization"; } }

        protected override void SerializeExemplar(IBenchmarkAdapter target)
        {
            target.Serialize(new MessageWithString() { Value = "Hello Protobuf World!"});
        }

        protected override void Deserialize(IBenchmarkAdapter target, out MessageWithString item)
        {
            target.Deserialize(out item);
        }
    }
}
