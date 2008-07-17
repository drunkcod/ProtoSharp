using ProtoSharp.Core;

namespace ProtoSharp.Tests.Messages
{
    class WithBytes
    {
        [Tag(1)]
        public byte[] Bytes { get; set; }
    }
}
