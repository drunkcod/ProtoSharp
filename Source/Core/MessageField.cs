using System;
using System.Reflection;
using System.Collections;
using System.Reflection.Emit;
using System.Collections.Generic;
using ProtoSharp.Core.MessageFields;

namespace ProtoSharp.Core
{
    public abstract class MessageField
    {
        public static MessageField Create(TagAttribute attr, PropertyInfo property)
        {
            IFieldIO fieldIO;
            if(!RepeatedFieldIO.TryCreate(property, out fieldIO))
                if(NullableFieldIO.IsNullable(property))
                    fieldIO = new NullableFieldIO(property);
                else
                    fieldIO = new FieldIO(property);

            return Create(attr, fieldIO);
        }

        protected MessageField(int tag, IFieldIO fieldIO, WireType wireType)
        {
            _header = tag << 3 | (int)wireType;
            _fieldIO = fieldIO;
        }

        public int Number { get { return _header >> 3; } }
        public int Header { get { return _header; } }
        public bool CanAppendWrite { get { return _fieldIO.CanCreateWriter && CanAppendWriteCore; } }
        public bool CanAppendRead { get { return _fieldIO.CanCreateReader && CanAppendReadCore; } }

        public abstract void AppendWriteField(ILGenerator il);

        public abstract void AppendReadField(ILGenerator il);

        public virtual void AppendGuard(ILGenerator il, MethodInfo getMethod, Label done)
        { }

        public virtual void AppendHeader(ILGenerator il)
        {
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldc_I4, _header);
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteVarint", new Type[] { typeof(uint) }));
        }

        public void AppendWriteBody(ILGenerator il)
        {
            _fieldIO.AppendWrite(il, this);            
        }

        public FieldReader<T> GetFieldReader<T>()
        {
            FieldReader<T> reader;
            if(CanAppendRead && _fieldIO.CreateReader<T>(this, out reader))
                return reader;
            throw new NotSupportedException();
        }

        protected virtual bool CanAppendWriteCore { get { return true; } }
        protected virtual bool CanAppendReadCore { get { return true; } }

        protected Type FieldType { get { return _fieldIO.FieldType; } }

        static MessageField Create(TagAttribute attr, IFieldIO io)
        {
            int tag = attr.Number;
            Type type = io.FieldType;
            if(type == typeof(Int32))
            {
                if(attr.UseZigZag)
                    return new MessageFieldSInt32(tag, io);
                if(attr.UseFixed)
                    return new MessageFieldFixedInt32(tag, io);
                return new MessageFieldInt32(tag, io);
            }

            if(type == typeof(Int64))
            {
                if(attr.UseFixed)
                    return new MessageFieldFixedInt64(tag, io);
                if(attr.UseZigZag)
                    return new MessageFieldSInt64(tag, io);
                return new MessageFieldInt64(tag, io);
            }

            if(type == typeof(string))
                return new MessageFieldString(tag, io);

            if(type == typeof(byte[]))
                return new MessageFieldBytes(tag, io);

            if(type == typeof(float))
                return new MessageFieldFloat(tag, io);

            if(type == typeof(double))
                return new MessageFieldDouble(tag, io);

            if(type == typeof(UInt32))
            {
                if(attr.UseFixed)
                    return new MessageFieldFixedUInt32(tag, io);
                return new MessageFieldUInt32(tag, io);
            }

            if(type == typeof(UInt64))
            {
                if(attr.UseFixed)
                    return new MessageFieldFixedUInt64(tag, io);
                return new MessageFieldUInt64(tag, io);
            }

            if(type == typeof(bool))
                return new MessageFieldBool(tag, io);

            if(type.IsEnum)
                return new MessageFieldEnum(tag, io);

            if(type == typeof(DateTime))
                return new MessageFieldDateTime(tag, io);

            if(type == typeof(Decimal))
                return new MessageFieldDecimal(tag, io);

            if(type == typeof(Int16))
                if(attr.UseZigZag)
                    return new MessageFieldSInt16(tag, io);
                else
                    return new MessageFieldInt16(tag, io);
            if(type == typeof(UInt16))
                return new MessageFieldInt16(tag, io);

            return new MessageFieldObject(tag, io);
        }

        int _header;
        IFieldIO _fieldIO;
    }
}
