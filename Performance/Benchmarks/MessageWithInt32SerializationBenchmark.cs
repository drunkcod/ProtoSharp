using System;
using System.Collections.Generic;
using System.Diagnostics;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance.Benchmarks
{
    class MessageWithInt32SerializationBenchmark
    {
        public MessageWithInt32SerializationBenchmark(int count, int seed, int iterations)
        {
            var rand = new Random(seed);
            for(int i = 0; i != count; ++i)
                _items.Add(new MessageWithInt32() { Value = rand.Next(0, 1 << 20) });
            _iterations = iterations;
        }

        public BenchmarkResult RunTest(string name, IBenchmarkAdapter target)
        {
            var min = TimeSpan.MaxValue;
            for(int i = 0; i != _iterations; ++i)
            {
                target.Reset();
                var stopwatch = Stopwatch.StartNew();
                _items.ForEach(target.Serialize);
                stopwatch.Stop();
                if(stopwatch.Elapsed < min)
                    min = stopwatch.Elapsed;
            }
            return new BenchmarkResult(name, min);
        }

        List<MessageWithInt32> _items = new List<MessageWithInt32>();
        int _iterations;
    }
}
