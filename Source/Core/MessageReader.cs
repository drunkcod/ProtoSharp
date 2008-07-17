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

        public MessageReader(byte[] message, int offset, int length)
        {
            _message = message;
            _offset = offset;
            _end = offset + length;
        }

        public MessageReader(byte[] message) : this(message, 0, message.Length) { }

        public int Position { get { return _offset; } }

        public event EventHandler FieldMissing;

        public MessageReader CreateSubReader(int length)
        {
            var subReader = new MessageReader(_message, _offset, length);
            _offset += length;
            return subReader;
        }

        public int ReadVarint32()
        {
            int value = 0;
            int shiftBits = 0;
            int bits;   
            do
            {
                bits = _message[_offset++];
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
                bits = _message[_offset++];
                value |= (bits & 0x7F) << shiftBits;
                shiftBits += 7;
            } while(bits > 0x7F);
            return value;
        }

        public int ReadFixedInt32()
        {
            _offset += 4;
            return _message[_offset - 4]
                | _message[_offset - 3] << 8
                | _message[_offset - 2] << 16
                | _message[_offset - 1] << 24;
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
            float value = new BinaryReader(new MemoryStream(_message, _offset, 4)).ReadSingle();
            _offset += 4;
            return value;
        }

        public double ReadDouble()
        {
            double value = new BinaryReader(new MemoryStream(_message, _offset, 8)).ReadDouble();
            _offset += 8;
            return value;
        }

        public string ReadString()
        {
            int length = ReadVarint32();
            string value = Encoding.UTF8.GetString(_message, _offset, length);
            _offset += length;
            return value;
        }

        public byte[] ReadBytes()
        {
            int length = ReadVarint32();
            byte[] value = new byte[length];
            Array.Copy(_message, _offset, value, 0, length);
            _offset += length;
            return value;
        }

        public MessageTag ReadMessageTag()
        {
            return new MessageTag(ReadVarint32());
        }

        public object ReadMessage(Type messageType)
        {
            return ReadMessage(messageType, _message.Length - _offset);
        }

        object ReadMessage(Type messageType, int length)
        {
            var fields = new Dictionary<int, MessageField>();
            Message.ForEachField(messageType,
                field => fields.Add(field.Tag, field));

            var obj = CreateDefault(messageType);
            var stop = _offset + length;
            while(Position != stop)
            {
                var tag = ReadMessageTag();
                MessageField field;
                if(!fields.TryGetValue(tag.Tag, out field))
                {
                    if(FieldMissing != null)
                        FieldMissing(this, EventArgs.Empty);
                    continue;
                }
                field.Read(obj, this);
            }
            return obj;
        }

        public T Read<T>() where T : class, new(){ return ReadMessage(typeof(T)) as T; }

        static object CreateDefault(Type type)
        {
            return type.GetConstructor(Type.EmptyTypes).Invoke(null);
        }

        byte[] _message;
        int _offset;
        int _end;
    }
}
