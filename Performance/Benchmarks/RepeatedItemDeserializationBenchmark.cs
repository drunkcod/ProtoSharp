using System;
using System.Collections.Generic;
using System.Text;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance.Benchmarks
{
    class RepeatedItemDeserializationBenchmark : DeserializationBenchmarkBase<MessageWithRepeatedItem>
    {
        public const int Items = 2000;

        public RepeatedItemDeserializationBenchmark(int iterations) : base(iterations) { }

        protected override string Name
        {
            get { return "List<int> Deserialization"; }
        }

        protected override void SerializeExemplar(IBenchmarkAdapter target)
        {            
            var data = new List<int>();
            Random rand = new Random(20080723);
            for(int i = 0; i != Items; ++i)
                data.Add(rand.Next(0, 1 << 20));
            target.Serialize(new MessageWithRepeatedItem(){ Value = data });
        }

        protected override void Deserialize(IBenchmarkAdapter target, out MessageWithRepeatedItem item)
        {
            target.Deserialize(out item);
        }
    }
}
