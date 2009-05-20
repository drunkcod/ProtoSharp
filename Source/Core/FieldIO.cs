using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    public delegate void FieldWriter<T>(T source, MessageWriter writer);
    public delegate void FieldReader<T>(T target, MessageReader reader);

    public interface  IFieldIO
    {
        Type FieldType { get; }
        bool CanCreateWriter { get; }
        bool CanCreateReader { get; }
        bool CreateReader<T>(MessageField field, out FieldReader<T> reader);
        void AppendWrite(ILGenerator il, MessageField field);
        void AppendRead(ILGenerator il, MessageField fiel);
    }

    class FieldIO : FieldIOBase
    {
        public FieldIO(PropertyInfo property): base(property){}

        public override void AppendWrite(ILGenerator il, MessageField field)
        {
            var done = il.DefineLabel();
            field.AppendGuard(il, property.GetGetMethod(), done);

            field.AppendHeader(il);

            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Call, property.GetGetMethod());
            field.AppendWriteField(il);
            il.Emit(OpCodes.Pop);
            il.MarkLabel(done);
        }

        public override void AppendRead(ILGenerator il, MessageField field)
        {
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ldarg_1);
            field.AppendReadField(il);
            il.Emit(OpCodes.Call, property.GetSetMethod());
        }
    }
}
