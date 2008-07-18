using System;
using System.Reflection;
using System.Collections;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    public class MessageField
    {
        public static MessageField Create(TagAttribute attr, PropertyInfo property)
        {
            IFieldIO fieldIO;
            if(!RepeatedFieldIO.TryCreate(property, out fieldIO))
                fieldIO = new FieldIO(property);
            return Create(attr, property.PropertyType, fieldIO);
        }

        protected MessageField(int tag, IFieldIO fieldIO, WireType wireType)
        {
            _tag = tag;
            _wireType = wireType;
            _fieldWriter = (source, writer) =>
            {
                _fieldWriter = fieldIO.CreateWriter(this);
                if(_fieldWriter == null)
                    _fieldWriter = (s, w) =>
                    fieldIO.Read(s, x =>
                    {
                        w.WriteHeader(Tag, WireType);
                        DoWrite(x, w);
                    });
                _fieldWriter(source, writer);
            };
            _fieldIO = fieldIO;
        }

        public int Tag { get { return _tag; } }
        public WireType WireType { get { return _wireType; } }

        public virtual bool AppendWrite(ILGenerator il)
        {
            return false;
        }

        public void Read(object target, MessageReader reader)
        {
            _fieldIO.Write(target, DoRead(reader));
        }
        public void Write(object source, MessageWriter writer)
        {
            _fieldWriter(source, writer);
        }

        protected virtual object DoRead(MessageReader reader)
        {
            return reader.CreateSubReader(reader.ReadVarint32()).
                   ReadMessage(FieldType);
        }

        protected virtual void DoWrite(object value, MessageWriter writer)
        {
            writer.WriteObject(value);
        }

        Type FieldType { get { return _fieldIO.FieldType; } }

        static MessageField Create(TagAttribute attr, Type type, IFieldIO fieldIO)
        {
            int tag = attr.Number;
            if(type == typeof(Int32))
            {
                if(attr.UseZigZag)
                    return new MessageFieldSInt32(tag, fieldIO);
                if(attr.UseFixed)
                    return new MessageFieldFixedInt32(tag, fieldIO);
                return new MessageFieldInt32(tag, fieldIO);
            }

            if(type == typeof(Int64))
            {
                if(attr.UseFixed)
                    return new MessageFieldFixedInt64(tag, fieldIO);
                if(attr.UseZigZag)
                    return new MessageFieldSInt64(tag, fieldIO);
                return new MessageFieldInt64(tag, fieldIO);
            }

            if(type == typeof(string))
                return new MessageFieldString(tag, fieldIO);

            if(type == typeof(byte[]))
                return new MessageFieldBytes(tag, fieldIO);

            if(type == typeof(float))
                return new MessageFieldFloat(tag, fieldIO);

            if(type == typeof(double))
                return new MessageFieldDouble(tag, fieldIO);

            if(type == typeof(UInt32))
            {
                if(attr.UseFixed)
                    return new MessageFieldFixedUInt32(tag, fieldIO);
                return new MessageFieldUInt32(tag, fieldIO);
            }

            if(type == typeof(UInt64))
            {
                if(attr.UseFixed)
                    return new MessageFieldFixedUInt64(tag, fieldIO);
                return new MessageFieldUInt64(tag, fieldIO);
            }

            if(type == typeof(bool))
                return new MessageFieldBool(tag, fieldIO);

            if(type.IsEnum)
                return new MessageFieldInt32(tag, fieldIO);

            return new MessageField(tag, fieldIO, WireType.String);
        }

        int _tag;
        WireType _wireType;
        FieldWriter _fieldWriter;
        IFieldIO _fieldIO;
    }
}
