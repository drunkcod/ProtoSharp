using System;

namespace ProtoSharp.Core
{
    public class ByteArrayReader : IByteReader
    {
        public ByteArrayReader(byte[] bytes, int offset, int length)
        {
            _bytes = bytes;
            _offset = offset;
            _end = offset + length;
        }

        public bool EndOfStream { get { return _offset == _end; } }

        public byte GetByte()
        {
            return _bytes[_offset++];
        }

        public ArraySegment<byte> GetBytes(int count)
        {
            var segment = new ArraySegment<byte>(_bytes, _offset, count);
            _offset += count;
            return segment;
        }

        public IByteReader GetByteReader(int length)
        {
            var subReader = new ByteArrayReader(_bytes, _offset, length);
            _offset += length;
            return subReader;
        }

        byte[] _bytes;
        int _offset;
        int _end;
    }
}
