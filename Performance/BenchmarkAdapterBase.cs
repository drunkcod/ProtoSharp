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
            Reset(0);
        }

        public void Reset(int length)
        {
            _memory.Position = 0;
            _memory.SetLength(length);
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

        public abstract void Deserialize(out MessageWithInt32 item);
        public abstract void Deserialize(out MessageWithUInt32 item);
        public abstract void Deserialize(out MessageWithSInt32 item);
        public abstract void Deserialize(out MessageWithFixed32 item);

        protected MemoryStream Memory { get { return _memory; } }

        MemoryStream _memory;
    }
}
