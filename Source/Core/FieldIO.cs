using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    public delegate void FieldWriter(object obj, MessageWriter writer);

    public interface  IFieldIO
    {
        void Read(object source, Action<object> action);
        void Write(object target, object value);
        Type FieldType { get; }
        bool CanCreateWriter { get; }
        bool CreateWriter(MessageField field, out FieldWriter writer);
        void AppendWrite(ILGenerator il, MessageField field);
    }

    class FieldIO : IFieldIO
    {
        public FieldIO(PropertyInfo property)
        {
            _property = property;
        }

        public bool CanCreateWriter { get { return true; } }

        public bool CreateWriter(MessageField field, out FieldWriter writer)
        {
            var builder = Message.BeginWriteMethod(_property.DeclaringType);
            AppendWrite(builder.GetILGenerator(), field);
            writer = Message.EndWriteMethod(builder);
            return true;
        }

        public void AppendWrite(ILGenerator il, MessageField field)
        {
            var done = il.DefineLabel();
            field.AppendGuard(il, _property.GetGetMethod(), done);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldc_I4, field.Header);
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteVarint", new Type[] { typeof(uint) }));

            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Call, _property.GetGetMethod());
            field.AppendWriteField(il);
            il.MarkLabel(done);
        }

        public void Read(object source, Action<object> action) 
        { 
            action(_property.GetValue(source, null));
        }

        public void Write(object target, object value)
        {
            _property.SetValue(target, value, null);
        }

        public Type FieldType { get { return _property.PropertyType; } }

        PropertyInfo _property;
    }
}
