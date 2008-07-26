using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    public abstract class FieldIOBase : IFieldIO
    {
        public virtual Type FieldType { get { return _property.PropertyType; } }

        public bool CanCreateWriter { get { return true; } }
        public bool CanCreateReader { get { return true; } }

        public bool CreateWriter(MessageField field, out FieldWriter writer)
        {
            var builder = Message.BeginWriteMethod(_property.DeclaringType, typeof(object));
            AppendWrite(builder.GetILGenerator(), field);
            writer = Message.EndMethod<FieldWriter>(builder);
            return true;
        }

        public bool CreateWriter<T>(MessageField field, out FieldWriter<T> writer)
        {
            var builder = Message.BeginWriteMethod(_property.DeclaringType, typeof(T));
            AppendWrite(builder.GetILGenerator(), field);
            writer = Message.EndMethod<FieldWriter<T>>(builder);
            return true;
        }

        public bool CreateReader(MessageField field, out FieldReader reader)
        {
            var builder = Message.BeginReadMethod(_property.DeclaringType, typeof(object));
            AppendRead(builder.GetILGenerator(), field);
            reader = Message.EndMethod<FieldReader>(builder);
            return true;
        }

        public bool CreateReader<T>(MessageField field, out FieldReader<T> reader)
        {
            var builder = Message.BeginReadMethod(_property.DeclaringType, typeof(T));
            AppendRead(builder.GetILGenerator(), field);
            reader = Message.EndMethod<FieldReader<T>>(builder);
            return true;
        }

        public abstract void Read(object target, object value);
        public abstract void AppendWrite(ILGenerator il, MessageField field);
        public abstract void AppendRead(ILGenerator il, MessageField field);

        protected FieldIOBase(PropertyInfo property)
        {
            _property = property;
        }

        static protected void AppendWriteHeader(ILGenerator il, MessageField field)
        {
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldc_I4, field.Header);
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteVarint", new Type[] { typeof(uint) }));
        }

        protected PropertyInfo _property;
    }
}
