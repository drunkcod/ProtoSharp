namespace ProtoSharp.Core
{
    using System;
    using System.Reflection;
    using System.Collections;
    using System.Reflection.Emit;
    using System.Collections.Generic;
    using ProtoSharp.Core.MessageFields;

    public class MessageField
    {
        public static MessageField Create(TagAttribute attr, PropertyInfo property)
        {
            return Create(attr, CreateFieldIO(property));
        }

        static IFieldIO CreateFieldIO(PropertyInfo property)
        {
            IFieldIO fieldIO;
            if (RepeatedFieldIO.TryCreate(property, out fieldIO) 
                || NullableFieldIO.TryCreate(property, out fieldIO))
                return fieldIO;
            return new FieldIO(property);
        }

        protected MessageField(int tag, IFieldIO fieldIO)
        {
            _tag = tag;
            _fieldIO = fieldIO;
        }

        public int Number { get { return _tag; } }
        public int Header { get { return _tag << 3 | (int)WireType; } }
        public bool CanAppendWrite { get { return _fieldIO.CanCreateWriter && CanAppendWriteCore; } }
        public bool CanAppendRead { get { return _fieldIO.CanCreateReader && CanAppendReadCore; } }

        protected virtual WireType WireType { get { return WireType.Varint; } }

        public virtual void AppendWriteField(ILGenerator il)
        {
            il.Call<MessageWriter>("Write" + FieldType.Name, FieldType);
        }

        public virtual void AppendReadField(ILGenerator il)
        {
            il.Call<MessageReader>("Read" + FieldType.Name);
        }

        public virtual void AppendGuard(ILGenerator il, MethodInfo getMethod, Label done)
        { }

        public virtual void AppendHeader(ILGenerator il)
        {
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldc_I4, Header);
            il.Call<MessageWriter>("WriteVarint", typeof(uint));
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
            if (attr.UseFixed)
                return CreateFixed(tag, io);
            if (attr.UseZigZag)
                return CreateZigZag(tag, io);

            Type type = io.FieldType;
            if(type.EqualsAny(typeof(Int32), typeof(Int64), typeof(bool), typeof(UInt32),  typeof(UInt64)))
                return new MessageFieldVarint(tag, io);

            if (type == typeof(string))
                return new MessageFieldString(tag, io);

            if(type == typeof(byte[]))
                return new MessageFieldBytes(tag, io);

            if(type == typeof(float))
                return new MessageFieldFixed(tag, io, WireType.Fixed32);

            if(type == typeof(double))
                return new MessageFieldFixed(tag, io, WireType.Fixed64);

            if(type.IsEnum)
                return new MessageFieldEnum(tag, io);

            if(type == typeof(DateTime) || type == typeof(Decimal))
                return new MessageField(tag, io);

            if (type == typeof(Int16) || type == typeof(UInt16))
                return new MessageFieldInt16(tag, io);

            return new MessageFieldObject(tag, io);
        }

        static MessageField CreateFixed(int tag, IFieldIO io)
        {
            Type type = io.FieldType;

            if (type == typeof(Int32) || type == typeof(UInt32))
                return new MessageFieldFixed(tag, io, WireType.Fixed32);

            if (type == typeof(Int64) || type == typeof(UInt64))
                return new MessageFieldFixed(tag, io, WireType.Fixed64);

            throw new NotSupportedException(string.Format("Unsupported field type \"{0}\"", type));
        }

        static MessageField CreateZigZag(int tag, IFieldIO io)
        {
            Type type = io.FieldType;

            if (type == typeof(Int32))
                    return new MessageFieldZigZagInt32(tag, io);

            if (type == typeof(Int64))
                return new MessageFieldZigZagInt64(tag, io);

            if (type == typeof(Int16))
                return new MessageFieldZigZagInt16(tag, io);

            throw new NotSupportedException(string.Format("Unsupported field type \"{0}\"", type));
        }

        int _tag;
        IFieldIO _fieldIO;
    }
}
