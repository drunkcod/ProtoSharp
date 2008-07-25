using System;
using System.Text;
using ProtoSharp.Core;

namespace ProtoSharp.Tests.Messages
{
    class MessageWithDateTime
    {
        [Tag(1)]
        public DateTime TimeStamp { get; set; }
    }
}
