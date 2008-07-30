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
            MemoryStream output = new MemoryStream();
            new MessageWriter(output).WriteMessage(message);
            return output.ToArray();
        }

        public MessageWriter(Stream output)
        {
            _writer = new BinaryWriter(output);
        }

        public MessageWriter WriteVarint(int value)
        {
            if(value < 0)
                return WriteVarint((UInt64)value);
            return WriteVarint((uint)value);
        }
        public MessageWriter WriteVarint(uint value)
            {//Special case for performance, single byte is *very* common.
            if(value < 0x80)
                _writer.Write((byte)value);
            else if(value < (1 << 14))
            {
                uint low = value & 0x7f;
                uint hi = value & 0x3F80;
                _writer.Write((UInt16)(hi << 1 | low | 0x80));
            }
            else
                do
                {
                    byte bits = (byte)(value & 0x7f);
                    value >>= 7;
                    if(value > 0)
                        bits |= 0x80;
                    _writer.Write(bits);
                } while(value != 0);
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
                _writer.Write(bits);
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

        public void WriteMessage<T>(T message)
        {
            SerializerHelper<T>.FieldWriter(message, this);
        }

        public MessageWriter WriteFixed(int value) { _writer.Write(value); return this; }

        public MessageWriter WriteFixed(uint value) { _writer.Write(value); return this; }

        public MessageWriter WriteFixed(float value) { _writer.Write(value); return this; }

        public MessageWriter WriteFixed(long value) { _writer.Write(value); return this; }

        public MessageWriter WriteFixed(ulong value) { _writer.Write(value); return this; }

        public MessageWriter WriteFixed(double value) { _writer.Write(value); return this; }

        public MessageWriter WriteDateTime(DateTime date)
        {
            return WriteZigZag(UnixTime.From(date));
        }

        public MessageWriter WriteObject<T>(T obj)
        {
            var embedded = new MemoryStream();
            var writer = new MessageWriter(embedded);
            writer.WriteMessage(obj);
            return WriteBytes(embedded.GetBuffer(), (int)embedded.Length);
        }

        public MessageWriter WriteHeader(int tag, WireType wireType)
        {
            return WriteVarint(new MessageTag(tag, wireType).Value);
        }

        BinaryWriter _writer;
    }
}
