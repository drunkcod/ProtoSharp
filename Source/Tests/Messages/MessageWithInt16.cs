using System;
using ProtoSharp.Core;

namespace ProtoSharp.Tests.Messages
{
    class MessageWithInt16
    {
        [Tag(1)]
        public Int16 Default { get; set; }
        [Tag(2, UseZigZag = true)]
        public Int16 ZigZag { get; set; }
        [Tag(3)]
        public UInt16 Unsigned { get; set; }
    }
}
