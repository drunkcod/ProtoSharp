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
        FieldWriter CreateWriter(MessageField field);
    }

    class FieldIO : IFieldIO
    {
        public FieldIO(PropertyInfo property)
        {
            _property = property;
        }

        public FieldWriter CreateWriter(MessageField field)
        {
            var writer = new DynamicMethod(string.Format("DynamicWrite{0}", _property.Name), null, new Type[]{ typeof(object), typeof(MessageWriter) }, true);

            var il = writer.GetILGenerator();
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldc_I4, field.Header);
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteVarint", new Type[]{ typeof(uint) }));

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, _property.DeclaringType);
            il.Emit(OpCodes.Call, _property.GetGetMethod());
            if(!field.AppendWrite(il))
                return null;
            il.Emit(OpCodes.Ret);
            return writer.CreateDelegate(typeof(FieldWriter)) as FieldWriter;
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
