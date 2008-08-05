using System.IO;

namespace ProtoSharp.Core
{
    public interface IObjectWriterStrategy
    {
        void Write<T>(MessageWriter target, int number, T value) where T : class;
    }

    class ObjectWriterStrategy : IObjectWriterStrategy
    {
        public void Write<T>(MessageWriter target, int number, T value) where T : class
        {
            target.WriteHeader(number, WireType.String);
            var embedded = new MemoryStream();
            Serializer.Serialize(new MessageWriter(embedded), value);
            target.WriteBytes(embedded.GetBuffer(), (int)embedded.Length);
        }
    }
}
