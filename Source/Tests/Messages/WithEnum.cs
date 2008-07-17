using ProtoSharp.Core;

namespace ProtoSharp.Tests.Messages
{
    class WithEnum
    {
        [Tag(1)]
        public WireType WireType { get; set; }
    }
}
