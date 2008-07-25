using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    public class NullableFieldIO : FieldIOBase
    {
        public static bool IsNullable(PropertyInfo property)
        {
            var type = property.PropertyType;
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public NullableFieldIO(PropertyInfo property) : base(property)
        {
            if(!IsNullable(property))
                throw new NotSupportedException();
        }

        public override Type FieldType { get { return _property.PropertyType.GetGenericArguments()[0]; } }

        public override void Read(object target, object value)
        {
            throw new NotImplementedException();
        }

        public override void AppendWrite(ILGenerator il, MessageField field)
        {
            var done = il.DefineLabel();
            field.AppendGuard(il, _property.GetGetMethod(), done);
            AppendWriteHeader(il, field);

            var tmp = il.DeclareLocal(typeof(Nullable<>).MakeGenericType(FieldType));

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Call, _property.GetGetMethod());
            il.Emit(OpCodes.Stloc, tmp.LocalIndex);
            il.Emit(OpCodes.Ldloca, tmp.LocalIndex);
            il.Emit(OpCodes.Call, typeof(Nullable<>).MakeGenericType(FieldType).GetProperty("Value").GetGetMethod());
            field.AppendWriteField(il);
            
            il.MarkLabel(done);
        }

        public override void AppendRead(ILGenerator il, MessageField field)
        {
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ldarg_1);
            field.AppendReadField(il);
            il.Emit(OpCodes.Newobj, typeof(Nullable<>).MakeGenericType(FieldType).GetConstructor(new Type[] { FieldType }));            
            il.Emit(OpCodes.Call, _property.GetSetMethod());
        }
    }
}
