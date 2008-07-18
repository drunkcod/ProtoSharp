using ProtoSharp.Core;

namespace ProtoSharp.Tests.Messages
{
    public class WithEnum
    {
        [Tag(1)]
        public WireType WireType { get; set; }
    }
}
