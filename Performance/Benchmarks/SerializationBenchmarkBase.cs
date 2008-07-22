using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProtoSharp.Performance.Benchmarks
{
    abstract class SerializationBenchmarkBase<T> : IBenchmark
    {
        public SerializationBenchmarkBase(int count, int seed, int iterations)
        {
            var rand = new Random(seed);
            for(int i = 0; i != count; ++i)
                _items.Add(Generate(rand));
            _iterations = iterations;
        }

        public BenchmarkResult Run(IBenchmarkAdapter target)
        {
            var min = TimeSpan.MaxValue;
            for(int i = 0; i != _iterations; ++i)
            {
                target.Reset();
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    _items.ForEach(x => Serialize(target, x));
                }
                catch(Exception)
                {
                    return new BenchmarkResult(Name, TimeSpan.Zero, 0);
                }
                stopwatch.Stop();
                if(stopwatch.Elapsed < min)
                    min = stopwatch.Elapsed;
            }
            return new BenchmarkResult(Name, min, target.BytesUsed);
        }

        protected abstract string Name { get; }
        protected abstract void Serialize(IBenchmarkAdapter target, T value);
        protected abstract T Generate(Random rand);

        List<T> _items = new List<T>();
        int _iterations;

    }
}
