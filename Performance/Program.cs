using System;

using System.Diagnostics;
using System.IO;
using ProtoSharp.Core;
using System.Collections.Generic;
using ProtoBuf;
using System.Runtime.Serialization;

namespace ProtoSharp.Performance
{
    [Serializable, DataContract]
    class MessageWithInt32
    {
        [Tag(1)]
        [ProtoMember(1, Name = "a", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int Value { get; set; }
    }

    class TestResult
    {
        public TestResult(string name, TimeSpan elapsed)
        {
            _name = name;
            _elapsed = elapsed;
        }

        public string Name { get { return _name; } }
        public TimeSpan Elapsed { get { return _elapsed; } }

        string _name;
        TimeSpan _elapsed;
    }

    class MessageWithInt32SerializationTester
    {
        public MessageWithInt32SerializationTester(int count, int seed, int iterations)
        {            
            var rand = new Random(seed);
            for(int i = 0; i != count; ++i)
                _items.Add(new MessageWithInt32() { Value = rand.Next(0, 1 << 20) });
            _iterations = iterations;
        }

        public TestResult RunTest(string name, Action<MessageWithInt32> handleData)
        {
            var min = TimeSpan.MaxValue;
            for(int i = 0; i != _iterations; ++i)
            {
                var stopwatch = Stopwatch.StartNew();
                _items.ForEach(handleData);
                stopwatch.Stop();
                if(stopwatch.Elapsed < min)
                    min = stopwatch.Elapsed;
            }
            return new TestResult(name, min);
        }

        List<MessageWithInt32> _items = new List<MessageWithInt32>();
        int _iterations;
    }

    public static class Program
    {
        const int Count = 2000;
        const int Iterations = 2000;

        static readonly byte[] block = new byte[2 * Count * Iterations * sizeof(int)];
        static MemoryStream CreateMemoryStream()
        {
            return new MemoryStream(block);
        }

        public static void Main(string[] args)
        {
            var tester = new MessageWithInt32SerializationTester(Count, 20080718, Iterations);
            var results = new List<TestResult>();
            Console.WriteLine("Serializing {0} {1} {2} times.", 
                Count, 
                typeof(MessageWithInt32).Name, 
                Iterations);
            {//System.IO.BinaryWriter
                var binaryWriter = new BinaryWriter(CreateMemoryStream());
                results.Add(tester.RunTest("System.IO.BinaryWriter", x => binaryWriter.Write(x.Value)));
            }
            {//ProtoSharp.Core.MessageWriter.WriteVarint
                var writer = new MessageWriter(CreateMemoryStream());
                results.Add(tester.RunTest("ProtoSharp.MessageWriter.WriteVarint", x => writer.WriteVarint(x.Value)));
            }
            {//ProtoSharp.Core.MessageWriter.WriteMessage
                var writer = new MessageWriter(CreateMemoryStream());
                results.Add(tester.RunTest("ProtoSharp.MessageWriter.WriteMessage", writer.WriteMessage));
            }
            {//protobuf-net
                var output = CreateMemoryStream();
                results.Add(tester.RunTest("protbuf-net", x => Serializer.Serialize(x, output)));
            }

            results.Sort((x, y) => x.Elapsed.CompareTo(y.Elapsed));
            var baseline = results[0].Elapsed;
            results.ForEach(result =>
            {
                Console.WriteLine("{0}:\n\ttook {1} ({2}%).", result.Name, result.Elapsed, (int)(100 * result.Elapsed.TotalMilliseconds / baseline.TotalMilliseconds));
            });
        }
    }
}