using System;
using System.Collections.Generic;
using System.IO;
using ProtoSharp.Performance.Benchmarks;
using ProtoSharp.Performance.Messages;
using ProtoSharp.Core;
using ProtoBuf;
using System.Runtime.Serialization;
using DAL;
using System.Diagnostics;

namespace ProtoSharp.Performance
{
    //Borrowed from protobuf-net
    [ProtoContract, DataContract]
    public class Database
    {
        [ProtoMember(1), Tag(1), DataMember(Order = 1)]
        public List<Order> Orders { get; private set; }

        public Database()
        {
            Orders = new List<Order>();
        }
    }

    class WriteObjectStrategy : IObjectWriterStrategy
    {
        public WriteObjectStrategy()
        {
            _buffer = new MemoryStream();
            _writer = new MessageWriter(_buffer);
        }

        public void Write<T>(MessageWriter target, int number, T value) where T : class
        {
            target.WriteHeader(number, WireType.String);
            _buffer.Position = 0;
            ProtoSharp.Core.Serializer.Serialize(_writer, value);
            target.WriteBytes(_buffer.GetBuffer(), (int)_buffer.Length);
        }

        MemoryStream _buffer;
        MessageWriter _writer;
    }

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

        public List<BenchmarkResult> CollectAll(IEnumerable<IBenchmark> benchmarks)
        {
            var results = new List<BenchmarkResult>();
            foreach(var benchmark in benchmarks)
                results.Add(benchmark.Run(_target));
            return results;
        }

        string _name;
        IBenchmarkAdapter _target;
    }

    public static class Program
    {
        const int Count = 2000;
        const int Iterations = 2000;

        static readonly byte[] block = new byte[1 << 20];

        class CountingWriteObjectStrategy : IObjectWriterStrategy
        {
            public void Write<T>(MessageWriter target, int number, T value) where T : class
            {
                Begin(target, number);
                ++_count;
                End(target, number);
            }

            public int Count { get { return _count; } }
            public int MaxDepth { get { return _maxDepth; } }

            void Begin(MessageWriter target, int number)
            {
                ++_depth;
                _maxDepth = Math.Max(_depth, _maxDepth);
            }

            void End(MessageWriter target, int number)
            {
                --_depth;
            }

            int _count = 0;
            int _depth = 0;
            int _maxDepth = 0;
        }

        public static void Main(string[] args)
        {
            var benchmarks = new IBenchmark[]
            {
                new Int32SerializationBenchmark(Count, 20080718, Iterations),
                new Int32DeserializationBenchmark(Iterations),
                new UInt32SerializationBenchmark(Count, 20080721, Iterations),
                new UInt32DeserializationBenchmark(Iterations),
                new SInt32SerializationBenchmark(Count, 20080721, Iterations),
                new SInt32DeserializationBenchmark(Iterations),
                new Fixed32SerializationBenchmark(Count, 20080721, Iterations),
                new Fixed32DeserializationBenchmark(Iterations),
                new Fixed64SerializationBenchmark(Count, 20080721, Iterations),
                new Fixed64DeserializationBenchmark(Iterations),
                new StringSerializationBenchmark(Count, 20080721, Iterations),
                new StringDeserializationBenchmark(Iterations),
                new ByteArraySerializationBenchmark(Count, 20080721, Iterations),
                new ByteArrayDeserializationBenchmark(Iterations),
                new RepeatedItemSerializationBenchmark(Count, 20080722, Iterations),
                new RepeatedItemDeserializationBenchmark(Iterations),
                new PersonSerializationBenchmark(Iterations),
                new PersonDeserializationBenchmark(Iterations)
            };
            if(args.Length != 0 && args[0] == "compare")
            {
                var protoSharp = new BenchmarkTarget("MessageWriter", new MessageWriterAdapter(block)).CollectAll(benchmarks);
                var protoBufNet = new BenchmarkTarget("protobuf-net", new ProtoBufNetAdapter(block)).CollectAll(benchmarks);
                for(int i = 0; i != protoSharp.Count; ++i)
                {
                    var item = protoSharp[i];
                    if(item.Elapsed > protoBufNet[i].Elapsed)
                        Console.WriteLine("{0} took {1} ticks (target {2})", item.Name, item.Elapsed.Ticks, protoBufNet[i].Elapsed.Ticks);
                }
            }
            else if(args.Length != 0 && args[0] == "repeated")
            {
                Console.WriteLine("Took {0} ticks.", new RepeatedItemDeserializationBenchmark(Iterations).Run(new MessageWriterAdapter(block)).Elapsed.Ticks);
            }
            else if(args.Length != 0 && args[0] == "northwind")
            {
                var northwindBytes = File.ReadAllBytes("nwind.proto.bin");
                var db = MessageReader.Read<Database>(northwindBytes);

                var counter = new CountingWriteObjectStrategy();
                ProtoSharp.Core.Serializer.Serialize(new MessageWriter(Stream.Null, counter), db);
                Console.WriteLine("Writing {0} sub messages with max depth of {1}.", counter.Count, counter.MaxDepth);

                var pbn = TimeSpan.MaxValue.Ticks;
                for(int i = 0; i != 100; ++i)
                {
                    var time = Stopwatch.StartNew();
                    ProtoBuf.Serializer.Serialize(Stream.Null, db);
                    time.Stop();
                    pbn = Math.Min(pbn, time.ElapsedTicks);
                }
                Console.WriteLine("protobuf-net:{0} ticks.", pbn);

                var ps = TimeSpan.MaxValue.Ticks;
                for(int i = 0; i != 100; ++i)
                {
                    var time = Stopwatch.StartNew();
                    ProtoSharp.Core.Serializer.Serialize(Stream.Null, db);
                    time.Stop();
                    ps = Math.Min(ps, time.ElapsedTicks);
                }
                Console.WriteLine("proto#      :{0} ticks.", ps);

                var protoSharpBufferReuse = TimeSpan.MaxValue.Ticks;
                for(int i = 0; i != 100; ++i)
                {
                    var time = Stopwatch.StartNew();
                    ProtoSharp.Core.Serializer.Serialize(new MessageWriter(Stream.Null, new WriteObjectStrategy()), db);
                    time.Stop();
                    protoSharpBufferReuse = Math.Min(protoSharpBufferReuse, time.ElapsedTicks);
                }
                Console.WriteLine("proto#      :{0} ticks.", protoSharpBufferReuse);

                var protoSharpGroups = TimeSpan.MaxValue.Ticks;
                for(int i = 0; i != 100; ++i)
                {
                    var time = Stopwatch.StartNew();
                    ProtoSharp.Core.Serializer.Serialize(new MessageWriter(Stream.Null, new GroupEncodingObjectWriterStrategy()), db);
                    time.Stop();
                    protoSharpGroups = Math.Min(protoSharpGroups, time.ElapsedTicks);
                }
                var tmp = new MemoryStream();
                ProtoSharp.Core.Serializer.Serialize(new MessageWriter(tmp, new GroupEncodingObjectWriterStrategy()), db);
                tmp.Position = 0;
                var groupDb = ProtoSharp.Core.Serializer.Deserialize<Database>(new MessageReader(tmp));

                var objectCount = 0;
                db.Orders.ForEach(x => objectCount += 1 + x.Lines.Count);
                DbMetrics("plain", db);
                DbMetrics("group", groupDb);
                var groupDbCount = 0;
                groupDb.Orders.ForEach(x => groupDbCount += 1 + x.Lines.Count);

                var roundtripBytes = MessageWriter.Write(groupDb);
                Console.WriteLine("{0}({1}) {2}({3})", MessageWriter.Write(db).Length, objectCount, roundtripBytes.Length, groupDbCount);
                Console.WriteLine("proto#      :{0} ticks. {1} bytes", protoSharpGroups, tmp.Position);

            }
            else
            {
                Array.ForEach(new BenchmarkTarget[]
                {
                    new BenchmarkTarget("System.IO.BinaryWriter", new BinaryWriterAdapter(block)),
                    new BenchmarkTarget("MessageWriter(Raw)", new MessageWriterRawAdapter(block)),
                    new BenchmarkTarget("MessageWriter", new MessageWriterAdapter(block)),
                    new BenchmarkTarget("protobuf-net", new ProtoBufNetAdapter(block))
                }, x => x.RunAll(benchmarks));
            }
        }

        static void DbMetrics(string caption, Database database)
        {
            int orders = database.Orders.Count;
            int lines = TotalLines(database);
            int totalQty = TotalQuantity(database);
            decimal totalValue = TotalValue(database);
            Console.WriteLine("{0}\torders {1}; lines {2}; units {3}; value {4:C}",
                caption, orders, lines, totalQty, totalValue);
        }

        static int TotalLines(Database db)
        {
            return Sum(db.Orders, (x, sum) => sum + x.Lines.Count, 0);
        }

        static int TotalQuantity(Database db)
        {
            return Sum(db.Orders, (x, sum) => sum + Sum(x.Lines, (line, s2) => s2 + line.Quantity, 0), 0);
        }

        static decimal TotalValue(Database db)
        {
            return Sum(db.Orders, (x, sum) => sum + Sum(x.Lines, (line, s2) => s2 + line.Quantity * line.UnitPrice, Decimal.Zero), Decimal.Zero);
        }

        static T Sum<T, U>(IEnumerable<U> items, Func<U, T, T> op, T start)
        {
            foreach(var x in items)
                start = op(x, start);
            return start;
        }
    }
}