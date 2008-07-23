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
            return Create(attr, fieldIO);
        }

        protected MessageField(int tag, IFieldIO fieldIO, WireType wireType)
        {
            _header = tag << 3 | (int)wireType;
            _fieldIO = fieldIO;
        }

        public int Tag { get { return _header >> 3; } }
        public int Header { get { return _header; } }
        public bool CanAppendWrite { get { return _fieldIO.CanCreateWriter && CanAppendWriteCore; } }
        public bool CanAppendRead { get { return _fieldIO.CanCreateReader && CanAppendReadCore; } }

        public virtual void AppendWriteField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteObject", new Type[] { typeof(object) }));
        }

        public virtual void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadVarint32"));
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("CreateSubReader"));
            il.Emit(OpCodes.Ldtoken, FieldType);
            il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadMessage"));
            il.Emit(OpCodes.Castclass, FieldType);
        }

        public virtual void AppendGuard(ILGenerator il, MethodInfo getMethod, Label done)
        { }

        public void Read(object target, MessageReader reader)
        {
            _fieldIO.Read(target, DoRead(reader));
        }

        public void AppendWriteBody(ILGenerator il)
        {
            _fieldIO.AppendWrite(il, this);            
        }

        public FieldWriter GetFieldWriter()
        {
            FieldWriter writer;
            if(CanAppendWrite && _fieldIO.CreateWriter(this, out writer))
                return writer;
            throw new NotSupportedException();
        }

        public FieldReader GetFieldReader()
        {
            FieldReader reader;
            if(CanAppendRead && _fieldIO.CreateReader(this, out reader))
                return reader;
            return Read;
        }

        protected virtual bool CanAppendWriteCore { get { return true; } }
        protected virtual bool CanAppendReadCore { get { return true; } }

        protected virtual object DoRead(MessageReader reader)
        {
            return reader.CreateSubReader(reader.ReadVarint32()).
                   ReadMessage(FieldType);
        }

        Type FieldType { get { return _fieldIO.FieldType; } }

        static MessageField Create(TagAttribute attr, IFieldIO fieldIO)
        {
            int tag = attr.Number;
            Type type = fieldIO.FieldType;
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

        int _header;
        IFieldIO _fieldIO;
    }
}
