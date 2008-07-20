using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ProtoSharp.Core
{
    public interface IByteReader
    {
        bool EndOfStream { get; }
        byte GetByte();
        ArraySegment<byte> GetBytes(int count);
        IByteReader GetByteReader(int lenght);
    }
    
    public class ByteReader : IByteReader
    {
        public ByteReader(Stream stream)
        {
            _stream = stream;
        }

        public bool EndOfStream
        {
            get { return _stream.Position == _stream.Length; }
        }

        public byte GetByte()
        {
            return (byte)_stream.ReadByte();
        }

        public ArraySegment<byte> GetBytes(int count)
        {
            var bytes = new byte[count];
            _stream.Read(bytes, 0, count);
            return new ArraySegment<byte>(bytes);
        }

        public IByteReader GetByteReader(int length)
        {
            var bytes = new byte[length];
            _stream.Read(bytes, 0, length);
            return new ByteArrayReader(bytes, 0, length);
        }

        Stream _stream;
    }
}
