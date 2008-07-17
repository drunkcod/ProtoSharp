
using ProtoSharp.Core;

namespace ProtoSharp.Tests.Messages
{
    class WithVarint32
    {
        [Tag(2, UseZigZag = true)]
        public int SInt32 { get { return _int32; } set { _int32 = value; } }
        [Tag(3)]
        public uint UInt32 { get { return _uint32; } set { _uint32 = value; } }

        int _int32;
        uint _uint32;
    }
}
