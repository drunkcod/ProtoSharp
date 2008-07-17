using System;
using ProtoSharp.Core;


namespace ProtoSharp.Tests.Messages
{
    class WithFixed64
    {
        [Tag(1, UseFixed = true)]
        public UInt64 UInt64{ get { return _uint64; } set { _uint64 = value; } }
        [Tag(2, UseFixed = true)]
        public Int64 Int64 { get { return _int64; } set { _int64 = value; } }
        [Tag(3)]
        public Double Double { get { return _double; } set { _double = value; } }

        ulong _uint64;
        long _int64;
        double _double;
    }
}
