using System;
using System.Collections.Generic;
using System.IO;
using ProtoSharp.Performance.Benchmarks;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance
{
    public static class Program
    {
        const int Count = 2000;
        const int Iterations = 2000;

        static readonly byte[] block = new byte[2 * Count * sizeof(int)];
        static MemoryStream CreateMemoryStream()
        {
            return new MemoryStream(block);
        }

        public static void Main(string[] args)
        {
            var tester = new MessageWithInt32SerializationBenchmark(Count, 20080718, Iterations);
            var results = new List<BenchmarkResult>();
            Console.WriteLine("Serializing {0} {1} {2} times.", 
                Count, 
                typeof(MessageWithInt32).Name, 
                Iterations);

            results.Add(tester.RunTest("System.IO.BinaryWriter", new BinaryWriterAdapter(block)));
            results.Add(tester.RunTest("ProtoSharp.MessageWriter.WriteVarint", new MessageWriterRawAdapter(block)));
            results.Add(tester.RunTest("ProtoSharp.MessageWriter.WriteMessage", new MessageWriterAdapter(block)));
            results.Add(tester.RunTest("protbuf-net", new ProtoBufNetAdapter(block)));

            results.Sort((x, y) => x.Elapsed.CompareTo(y.Elapsed));
            var baseline = results[0].Elapsed;
            results.ForEach(result =>
            {
                Console.WriteLine("{0}:\n\ttook {1}ticks ({2}%).", result.Name, result.Elapsed.Ticks, (int)(100 * result.Elapsed.TotalMilliseconds / baseline.TotalMilliseconds));
            });
        }
    }
}