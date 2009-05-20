using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    public abstract class FieldIOBase : IFieldIO
    {
        public virtual Type FieldType { get { return property.PropertyType; } }

        public bool CanCreateWriter { get { return true; } }
        public bool CanCreateReader { get { return true; } }

        public bool CreateReader<T>(MessageField field, out FieldReader<T> reader)
        {
            var builder = Message.BeginReadMethod(property.DeclaringType, typeof(T));
            AppendRead(builder.GetILGenerator(), field);
            reader = Message.EndMethod<FieldReader<T>>(builder);
            return true;
        }

        public abstract void AppendWrite(ILGenerator il, MessageField field);
        public abstract void AppendRead(ILGenerator il, MessageField field);
        public void AppendGetField(ILGenerator il) 
        {
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Call, property.GetGetMethod());
        }
        protected FieldIOBase(PropertyInfo property)
        {
            this.property = property;
        }

        protected PropertyInfo property;
    }
}
