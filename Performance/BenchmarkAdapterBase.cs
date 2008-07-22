using System.IO;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance
{
    abstract class BenchmarkAdapterBase : IBenchmarkAdapter
    {
        public BenchmarkAdapterBase(byte[] memory)
        {
            _memory = new MemoryStream(memory);
        }

        public int BytesUsed { get { return (int)_memory.Position; } }

        public void Reset()
        {
            _memory.Position = 0;
        }

        public abstract void Serialize(MessageWithInt32 item);
        public abstract void Serialize(MessageWithUInt32 item);
        public abstract void Serialize(MessageWithSInt32 item);
        public abstract void Serialize(MessageWithFixed32 item);
        public abstract void Serialize(MessageWithFixed64 item);
        public abstract void Serialize(MessageWithString item);
        public abstract void Serialize(MessageWithBytes item);
        public abstract void Serialize(MessageWithRepeatedItem item);
        public abstract void Serialize(Person item);

        protected MemoryStream Memory { get { return _memory; } }

        MemoryStream _memory;
    }
}
