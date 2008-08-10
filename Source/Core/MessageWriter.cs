using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


namespace ProtoSharp.Core
{
    public class MessageWriter
    {
        public static byte[] Write<T>(T message)
        {
            var output = new MemoryStream();
            Serializer.Serialize(new MessageWriter(output), message);
            return output.ToArray();
        }

        public MessageWriter(Stream output, IObjectWriterStrategy objectWriter)
        {
            _writeObject = objectWriter;
            _writer = output;
        }

        public MessageWriter(Stream output): this(output, new ObjectWriterStrategy()) {}

        public MessageWriter WriteVarint(int value)
        {
            if(value < 0)
                return WriteVarint((UInt64)value);
            return WriteVarint((uint)value);
        }
        public MessageWriter WriteVarint(uint value)
            {//Special case for performance, single byte is *very* common.
            if(value < 0x80)
                Write((byte)value);
            else if(value < (1 << 14))
            {
                uint low = value & 0x7f;
                uint hi = value & 0x3F80;
                Write((UInt16)(hi << 1 | low | 0x80));
            }
            else
            {
                do
                {
                    byte bits = (byte)(value & 0x7f);
                    value >>= 7;
                    if(value > 0)
                        bits |= 0x80;
                    Write(bits);
                } while(value != 0);
            }
            return this;
        }

        public MessageWriter WriteVarint(Int64 value) { return WriteVarint((UInt64)value); }
        public MessageWriter WriteVarint(UInt64 value)
        {
            do
            {
                byte bits = (byte)(value & 0x7f);
                value >>= 7;
                if(value > 0)
                    bits |= 0x80;
                Write(bits);
            } while(value != 0);
            return this;
        }

        public MessageWriter WriteVarint(bool value) { return WriteVarint(Convert.ToInt32(value)); }

        public MessageWriter WriteZigZag(int value)
        {
            return WriteVarint(value << 1 ^ value >> 31);
        }

        public MessageWriter WriteZigZag(Int64 value)
        {
            return WriteVarint(value << 1 ^ value >> 63);
        }

        public MessageWriter WriteString(string value)
        {
            if(value == null)
                return this;
            return WriteBytes(Encoding.UTF8.GetBytes(value));
        }

        public MessageWriter WriteBytes(byte[] value)
        {
            return WriteBytes(value, value.Length);
        }

        public MessageWriter WriteBytes(byte[] value, int length)
        {
            WriteVarint(length);
            _writer.Write(value, 0, length);
            return this;
        }

        public MessageWriter WriteFixed(int value) {  return WriteRaw(BitConverter.GetBytes( value)); }

        public MessageWriter WriteFixed(uint value) { return WriteRaw(BitConverter.GetBytes(value)); }

        public MessageWriter WriteFixed(float value) { return WriteRaw(BitConverter.GetBytes(value)); }

        public MessageWriter WriteFixed(long value) { return WriteRaw(BitConverter.GetBytes(value)); }

        public MessageWriter WriteFixed(ulong value) { return WriteRaw(BitConverter.GetBytes(value)); }

        public MessageWriter WriteFixed(double value) { return WriteRaw(BitConverter.GetBytes(value)); }

        public MessageWriter WriteDateTime(DateTime date)
        {
            return WriteZigZag(UnixTime.From(date));
        }

        public MessageWriter WriteObject<T>(T obj, int number) where T : class
        {
            _writeObject.Write<T>(this, number, obj);
            return this;
        }

        public MessageWriter WriteHeader(int tag, WireType wireType)
        {
            return WriteVarint(new MessageTag(tag, wireType).Value);
        }

        public int LengthVarint(long value)
        {
            return LengthVarint((ulong)value);
        }

        public int LengthVarint(ulong value)
        {
            int length = 0;
            do
            {
                ++length;
                value >>= 7;
            } while(value != 0);
            return length;
        }

        void Write(byte value)
        {
            _writer.WriteByte(value);
        }

        void Write(UInt16 value)
        {
            _writer.WriteByte((byte)(value & 0xff));
            _writer.WriteByte((byte)(value >> 8));
        }

        MessageWriter WriteRaw(byte[] bytes)
        {
            _writer.Write(bytes, 0, bytes.Length);
            return this;
        }

        Stream _writer;
        IObjectWriterStrategy _writeObject;
    }
}
