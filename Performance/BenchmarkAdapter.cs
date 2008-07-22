using System;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance
{
    interface IBenchmarkAdapter
    {
        int BytesUsed { get; }
        void Reset();
        void Serialize(MessageWithInt32 item);
        void Serialize(MessageWithUInt32 item);
        void Serialize(MessageWithSInt32 item);
        void Serialize(MessageWithFixed32 item);
        void Serialize(MessageWithFixed64 item);
        void Serialize(MessageWithString item);
        void Serialize(MessageWithBytes item);
        void Serialize(MessageWithRepeatedItem item);
        void Serialize(Person item);
    }
}
