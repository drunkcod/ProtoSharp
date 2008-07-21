using System;
using System.Collections.Generic;
using System.IO;
using ProtoSharp.Performance.Benchmarks;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance
{
    class BenchmarkTarget
    {
        public BenchmarkTarget(string name, IBenchmarkAdapter target)
        {
            _name = name;
            _target = target;
        }

        public void RunAll(IEnumerable<IBenchmark> benchmarks)
        {
            Console.WriteLine(_name);
            foreach(var benchmark in benchmarks)
            {
                var result = benchmark.Run(_target);
                Console.WriteLine("\t{0} took {1} ticks.", result.Name, result.Elapsed.Ticks);
            }
        }

        string _name;
        IBenchmarkAdapter _target;
    }


    public static class Program
    {
        const int Count = 2000;
        const int Iterations = 2000;

        static readonly byte[] block = new byte[2 * Count * sizeof(int)];

        public static void Main(string[] args)
        {
            var benchmarks = new IBenchmark[]
            {
                new Int32SerializationBenchmark(Count, 20080718, Iterations),
                new UInt32SerializationBenchmark(Count, 20080721, Iterations),
                new SInt32SerializationBenchmark(Count, 20080721, Iterations)
            };

            Array.ForEach(new BenchmarkTarget[]
            {
                new BenchmarkTarget("System.IO.BinaryWriter", new BinaryWriterAdapter(block)),
                new BenchmarkTarget("MessageWriter(Raw)", new MessageWriterRawAdapter(block)),
                new BenchmarkTarget("MessageWriter", new MessageWriterAdapter(block)),
                new BenchmarkTarget("protobuf-net", new ProtoBufNetAdapter(block))
            }, x => x.RunAll(benchmarks));
        }
    }
}