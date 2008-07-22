using System;
using System.Collections.Generic;
using System.IO;
using ProtoSharp.Performance.Benchmarks;
using ProtoSharp.Performance.Messages;
using ProtoSharp.Core;
using ProtoBuf;

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
                Console.WriteLine("\t{0} took {1} ticks used {2} bytes", result.Name, result.Elapsed.Ticks, result.BytesUsed);
            }
        }

        string _name;
        IBenchmarkAdapter _target;
    }

    public static class Program
    {
        const int Count = 2000;
        const int Iterations = 2000;

        static readonly byte[] block = new byte[1 << 20];

        public static void Main(string[] args)
        {
            var benchmarks = new IBenchmark[]
            {
                new Int32SerializationBenchmark(Count, 20080718, Iterations),
                new UInt32SerializationBenchmark(Count, 20080721, Iterations),
                new SInt32SerializationBenchmark(Count, 20080721, Iterations),
                new Fixed32SerializationBenchmark(Count, 20080721, Iterations),
                new Fixed64SerializationBenchmark(Count, 20080721, Iterations),
                new StringSerializationBenchmark(Count, 20080721, Iterations),
                new ByteArraySerializationBenchmark(Count, 20080721, Iterations),
                new RepeatedItemSerializationBenchmark(Count, 20080722, Iterations),
                new PersonSerializationBenchmark(Iterations)
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