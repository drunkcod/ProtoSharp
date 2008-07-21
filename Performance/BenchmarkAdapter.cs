using System;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance
{
    interface IBenchmarkAdapter
    {
        void Reset();
        void Serialize(MessageWithInt32 item);
        void Serialize(MessageWithUInt32 item);
    }
}
