using ProtoSharp.Core;

namespace ProtoSharp.Tests.Messages
{
    class WithBool
    {
        [Tag(1)]
        public bool Boolean { get; set; }
    }
}
