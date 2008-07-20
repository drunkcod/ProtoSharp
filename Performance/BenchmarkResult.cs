using System;

namespace ProtoSharp.Performance
{
    class BenchmarkResult
    {
        public BenchmarkResult(string name, TimeSpan elapsed)
        {
            _name = name;
            _elapsed = elapsed;
        }

        public string Name { get { return _name; } }
        public TimeSpan Elapsed { get { return _elapsed; } }

        string _name;
        TimeSpan _elapsed;
    }
}
