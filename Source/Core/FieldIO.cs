using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    public delegate void FieldWriter(object obj, MessageWriter writer);
    public delegate void FieldReader(object obj, MessageReader reader);

    public interface  IFieldIO
    {
        Type FieldType { get; }
        bool CanCreateWriter { get; }
        bool CanCreateReader { get; }
        bool CreateWriter(MessageField field, out FieldWriter writer);
        bool CreateReader(MessageField field, out FieldReader reader);
        void AppendWrite(ILGenerator il, MessageField field);
        void AppendRead(ILGenerator il, MessageField fiel);
        void Read(object target, object value);
    }

    class FieldIO : FieldIOBase
    {
        public FieldIO(PropertyInfo property): base(property){}

        public override void AppendWrite(ILGenerator il, MessageField field)
        {
            var done = il.DefineLabel();
            field.AppendGuard(il, _property.GetGetMethod(), done);
            AppendWriteHeader(il, field);

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Call, _property.GetGetMethod());
            field.AppendWriteField(il);
            il.MarkLabel(done);
        }

        public override void AppendRead(ILGenerator il, MessageField field)
        {
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ldarg_1);
            field.AppendReadField(il);
            il.Emit(OpCodes.Call, _property.GetSetMethod());
        }

        public override void Read(object target, object value)
        {
            _property.SetValue(target, value, null);
        }
    }
}
