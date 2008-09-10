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
        float GetFloat();
        ArraySegment<byte> GetAllBytes();
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

        public float GetFloat()
        {
            _stream.Read(_bytes, 0, sizeof(float));
            return BitConverter.ToSingle(_bytes, 0);
        }

        public ArraySegment<byte> GetAllBytes()
        {
            var bytesLeft = (int)(_stream.Length - _stream.Position);
            var bytes = AllocBytes(bytesLeft);
            _stream.Read(bytes, 0, bytesLeft);
            return new ArraySegment<byte>(bytes, 0, bytesLeft);
        }

        public ArraySegment<byte> GetBytes(int count)
        {
            var bytes = AllocBytes(count);
            _stream.Read(bytes, 0, count);
            return new ArraySegment<byte>(bytes, 0, count);
        }

        public IByteReader GetByteReader(int length)
        {
            var bytes = AllocBytes(length);
            _stream.Read(bytes, 0, length);
            return new ByteArrayReader(bytes, 0, length);
        }

        byte[] AllocBytes(int length)
        {
            if(length < _bytes.Length)
                return _bytes;
            return new byte[length];
        }

        Stream _stream;
        byte[] _bytes = new byte[128];
    }
}
