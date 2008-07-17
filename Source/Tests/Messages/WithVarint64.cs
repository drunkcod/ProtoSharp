
using ProtoSharp.Core;
using System;

namespace ProtoSharp.Tests.Messages
{
    class WithVarint64
    {
        [Tag(2, UseZigZag = true)]
        public Int64 SInt64 { get { return _signed; } set { _signed = value; } }
        [Tag(3)]
        public UInt64 UInt64 { get { return _unsigned; } set { _unsigned = value; } }

        Int64 _signed;
        UInt64 _unsigned;
    }
}