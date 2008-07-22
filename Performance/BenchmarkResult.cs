using System;

namespace ProtoSharp.Performance
{
    class BenchmarkResult
    {
        public BenchmarkResult(string name, TimeSpan elapsed, int bytesUsed)
        {
            _name = name;
            _elapsed = elapsed;
            _bytesUsed = bytesUsed;
        }

        public string Name { get { return _name; } }
        public TimeSpan Elapsed { get { return _elapsed; } }
        public int BytesUsed { get { return _bytesUsed; } }

        string _name;
        TimeSpan _elapsed;
        int _bytesUsed; 
    }
}
