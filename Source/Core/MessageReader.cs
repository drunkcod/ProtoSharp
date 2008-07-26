using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace ProtoSharp.Core
{
    public class MessageReader
    {
        public static T Read<T>(byte[] message) where T: class, new()
        { 
            return new MessageReader(message).Read<T>(); 
        }

        public static T Read<T>(Stream stream) where T : class, new()
        {
            return new MessageReader(new ByteReader(stream)).Read<T>();
        }

        public MessageReader(IByteReader bytes)
        {
            _bytes = bytes;
        }

        public MessageReader(byte[] message) : this(new ByteArrayReader(message, 0, message.Length)) { }

        public event EventHandler FieldMissing;

        public MessageReader CreateSubReader(int length)
        {
            return new MessageReader(_bytes.GetByteReader(length));
        }

        public bool ReadBool()
        {
            return ReadVarint32() != 0;
        }

        public int ReadVarint32()
        {
            int bits,value = _bytes.GetByte();
            if(value < 0x80)
                return value;

            int shiftBits = 7;
            value &= 0x7f;
            do
            {
                bits = _bytes.GetByte();
                if(shiftBits < 31)
                {
                    value |= (bits & 0x7F) << shiftBits;
                    shiftBits += 7;
                }
            } while(bits > 0x7F);
            return value;
        }

        public Int64 ReadVarint64()
        {
            Int64 value = 0;
            int shiftBits = 0;
            Int64 bits;
            do
            {
                bits = _bytes.GetByte();
                value |= (bits & 0x7F) << shiftBits;
                shiftBits += 7;
            } while(bits > 0x7F);
            return value;
        }

        public int ReadZigZag32()
        {
            var value = (uint)ReadVarint32();
            var mask = 0 - (value & 1);
            return (int)(value >> 1 ^ mask);
        }

        public Int64 ReadZigZag64()
        {
            UInt64 value = (UInt64)ReadVarint64();
            UInt64 mask = 0L - (value & 1);
            return (Int64)(value >> 1 ^ mask);
        }

        public int ReadFixedInt32()
        {
            return _bytes.GetByte()
                | _bytes.GetByte() << 8
                | _bytes.GetByte() << 16
                | _bytes.GetByte() << 24;
        }

        public uint ReadFixedUInt32()
        {
            return (uint)ReadFixedInt32();
        }

        public Int64 ReadFixedInt64()
        {
            Int64 lo = (long)ReadFixedUInt32(), hi = (long)ReadFixedUInt32();
            return lo | (hi << 32);
        }

        public UInt64 ReadFixedUInt64()
        {
            return (UInt64)ReadFixedInt64();
        }

        public float ReadFloat()
        {
            return _bytes.GetFloat();
        }

        public double ReadDouble()
        {
            return BitConverter.Int64BitsToDouble(ReadFixedInt64());
        }

        public string ReadString()
        {
            var bytes = _bytes.GetBytes(ReadVarint32());
            var value = Encoding.UTF8.GetString(bytes.Array, bytes.Offset, bytes.Count);
            return value;
        }

        public byte[] ReadBytes()
        {
            var bytes = _bytes.GetBytes(ReadVarint32());
            var value = new byte[bytes.Count];
            Array.Copy(bytes.Array, bytes.Offset, value, 0, bytes.Count);
            return value;
        }

        public DateTime ReadDateTime()
        {
            return UnixTime.ToDateTime(ReadVarint32());
        }

        public MessageTag ReadMessageTag()
        {
            return new MessageTag(ReadVarint32());
        }

        public T Read<T>() where T : class, new()
        {
            var target = new T();
            return Read<T>(target);
        }

        public T Read<T>(T target) where T : class
        {
            FieldReader<T> field = null;
            var helper = new SerializerHelper<T>();
            while(!_bytes.EndOfStream)
            {
                var tag = ReadVarint32();
                if(helper.TryGetFieldReader(tag, out field))
                    field(target, this);
                else
                    OnMissingField(EventArgs.Empty);
            }
            return target;
        }

        public void OnMissingField(EventArgs e)
        {
            if(FieldMissing != null)
                FieldMissing(this, EventArgs.Empty);
        }

        static object CreateDefault(Type type)
        {
            return type.GetConstructor(Type.EmptyTypes).Invoke(null);
        }

        static Dictionary<Type, Dictionary<int, FieldReader>> s_readerCache = new Dictionary<Type, Dictionary<int, FieldReader>>();

        IByteReader _bytes;
    }
}
