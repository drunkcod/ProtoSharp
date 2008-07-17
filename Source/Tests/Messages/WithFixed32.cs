using ProtoSharp.Core;

namespace ProtoSharp.Tests.Messages
{
    class WithFixed32
    {
        [Tag(1, UseFixed = true)]
        public int Int32 { get { return _int; } set { _int = value; } }
        [Tag(2, UseFixed = true)]
        public uint UInt32 { get { return _uint; } set { _uint = value; } }
        [Tag(3)]
        public float Float { get { return _float; } set { _float = value; } }

        int _int;
        uint _uint;
        float _float;
    }
}
