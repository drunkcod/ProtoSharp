using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;


namespace ProtoSharp.Core
{
    public class MessageWriter
    {
        public static byte[] Write(object message)
        {
            MemoryStream output = new MemoryStream();
            new MessageWriter(output).WriteMessage(message);
            return output.ToArray();
        }
        public MessageWriter(Stream output)
        {
            _writer = new BinaryWriter(output);
        }

        public void WriteVarint(int value)
        {
            if(value < 0)
                WriteVarint((UInt64)value);
            else
                WriteVarint((uint)value);
        }
        public void WriteVarint(uint value)
            {//Special case for performance, single byte is *very* common.
            if(value < 0x80)
                _writer.Write((byte)value);
            else if(value < (1 << 15))
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
        }

        public void WriteVarint(Int64 value) { WriteVarint((UInt64)value); }
        public void WriteVarint(UInt64 value)
        {
            do
            {
                byte bits = (byte)(value & 0x7f);
                value >>= 7;
                if(value > 0)
                    bits |= 0x80;
                _writer.Write(bits);
            } while(value != 0);
        }

        public void WriteZigZag(int value)
        {
            WriteVarint(value << 1 ^ value >> 31);
        }

        public void WriteString(string value)
        {
            WriteVarint(value.Length);
            _writer.Write(Encoding.UTF8.GetBytes(value));
        }

        public void WriteBytes(byte[] value)
        {
            WriteBytes(value, value.Length);
        }

        public void WriteBytes(byte[] value, int length)
        {
            WriteVarint(length);
            _writer.Write(value, 0, length);
        }

        public void WriteMessage(object obj)
        {
            FieldWriter writeFields;
            if(!_writerCache.TryGetValue(obj.GetType(), out writeFields))
            {
                Message.ForEachField(obj.GetType(), x => writeFields += x.GetFieldWriter());
                _writerCache.Add(obj.GetType(), writeFields);
            }
            writeFields(obj, this);
        }

        public void WriteFixed(int value) { _writer.Write(value); }

        public void WriteFixed(uint value) { _writer.Write(value); }

        public void WriteFixed(float value) { _writer.Write(value); }

        public void WriteFixed(long value) { _writer.Write(value); }

        public void WriteFixed(ulong value) { _writer.Write(value); }

        public void WriteFixed(double value) { _writer.Write(value); }

        public void WriteObject(object obj)
        {
            var embedded = new MemoryStream();
            var writer = new MessageWriter(embedded);
            writer.WriteMessage(obj);
            WriteBytes(embedded.GetBuffer(), (int)embedded.Length);
        }

        public void WriteHeader(int tag, WireType wireType)
        {
            WriteVarint(tag << 3 | (int)wireType);
        }

        BinaryWriter _writer;
        Dictionary<Type, FieldWriter> _writerCache = new Dictionary<Type, FieldWriter>();
    }
}
