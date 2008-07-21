using System;

namespace ProtoSharp.Performance.Benchmarks
{
    interface IBenchmark
    {
        BenchmarkResult Run(IBenchmarkAdapter target);
    }
}
