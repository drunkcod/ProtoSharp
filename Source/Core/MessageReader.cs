namespace ProtoSharp.Core
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Reflection;
    using System.IO;
    using ProtoSharp.Core.MessageFields;
    using ProtoSharp.Core.UnknownFields;

    public class MessageReader
    {
        public static T Read<T>(byte[] message) where T: class, new()
        {
            return new Serializer<T>().Deserialize(new MessageReader(message), new T(), new UnknownFieldCollection());
        }

        public MessageReader(Stream stream) : this(new ByteReader(stream)) { }
        public MessageReader(IByteReader bytes)
        {
            _bytes = bytes;
        }

        public MessageReader(params byte[] message) : this(new ByteArrayReader(message, 0, message.Length)) { }

        public bool ReadBoolean()
        {
            return ReadInt32() != 0;
        }

        public int ReadInt32()
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

        public UInt32 ReadUInt32() { return (UInt32)ReadInt32(); }

        public Int64 ReadInt64()
        {
            Int64 value = 0;
            int shiftBits = 0;
            Int64 bits;
            do
            {
                bits = _bytes.GetByte();
                value |= (bits & 0x7F) << shiftBits;
                shiftBits += 7;
            } while(bits > 0x7F && shiftBits < 64);
            return value;
        }

        public UInt64 ReadUInt64(){ return (UInt64)ReadInt64(); }

        public int ReadZigZag32()
        {
            var value = (uint)ReadInt32();
            var mask = 0 - (value & 1);
            return (int)(value >> 1 ^ mask);
        }

        public Int64 ReadZigZag64()
        {
            UInt64 value = (UInt64)ReadInt64();
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

        public float ReadFixedSingle()
        {
            return _bytes.GetFloat();
        }

        public double ReadFixedDouble()
        {
            return BitConverter.Int64BitsToDouble(ReadFixedInt64());
        }

        public string ReadString()
        {
            var bytes = _bytes.GetAllBytes();
            var value = Encoding.UTF8.GetString(bytes.Array, bytes.Offset, bytes.Count);
            return value;
        }

        public byte[] ReadBytes()
        {
            var bytes = _bytes.GetBytes(ReadInt32());
            var value = new byte[bytes.Count];
            Array.Copy(bytes.Array, bytes.Offset, value, 0, bytes.Count);
            return value;
        }

        public DateTime ReadDateTime()
        {
            return UnixTime.ToDateTime(ReadInt32());
        }

        public Decimal ReadDecimal()
        {
            return new Decimal(ReadInt64()) / DecimalField.Factor;
        }

        public MessageTag ReadMessageTag()
        {
            return new MessageTag(ReadInt32());
        }

        public bool TryReadMessageTag(ref MessageTag target)
        {
            if(_bytes.EndOfStream)
                return false;
            target = ReadMessageTag();
            return true;
        }

        public int ReadEnum(Type enumType)
        {
            var value = ReadInt32();
            if(!Enum.IsDefined(enumType, value))
                throw new UnknownEnumException(value);
            return value;
        }

        internal MessageReader CreateSubReader()
        {
            return new MessageReader(_bytes.GetByteReader(ReadInt32()));
        }

        internal T Read<T>() where T : class, new()
        {
            return new Serializer<T>().Deserialize(this, new T(), new UnknownFieldCollection());
        }

        IByteReader _bytes;
    }
}
