using System;
using ProtoSharp.Core;

namespace ProtoSharp.Tests.Messages
{
    class MessageWithDecimal
    {
        [Tag(1)]
        public Decimal Value { get; set; }
    }
}
