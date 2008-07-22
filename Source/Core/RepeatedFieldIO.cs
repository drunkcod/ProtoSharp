using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    public class RepeatedFieldIO : IFieldIO
    {
        public static bool TryCreate(PropertyInfo property, out IFieldIO io)
        {
            MethodInfo add = property.PropertyType.GetMethod("Add");
            MethodInfo getEnumerator = property.PropertyType.GetMethod("GetEnumerator", Type.EmptyTypes);
            if(add == null || getEnumerator == null)
            {
                io = null;
                return false;
            }
            io = new RepeatedFieldIO(property, add, getEnumerator);
            return true;
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
            var top = il.DefineLabel();
            var next = il.DefineLabel();
            var enumerator = il.DeclareLocal(typeof(IEnumerator));

            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Call, _property.GetGetMethod());
            il.Emit(OpCodes.Call, _getEnumerator);
            il.Emit(OpCodes.Box, _getEnumerator.ReturnType);
            il.Emit(OpCodes.Stloc, enumerator.LocalIndex);
            il.Emit(OpCodes.Br, next);
            il.MarkLabel(top);

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldc_I4, field.Header);
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteVarint", new Type[] { typeof(uint) }));

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldloc, enumerator.LocalIndex);
            il.Emit(OpCodes.Callvirt, typeof(IEnumerator).GetProperty("Current").GetGetMethod());
            il.Emit(OpCodes.Unbox_Any, FieldType);
            field.AppendWriteField(il);

            il.MarkLabel(next);
            il.Emit(OpCodes.Ldloc, enumerator.LocalIndex);
            il.Emit(OpCodes.Callvirt, typeof(IEnumerator).GetMethod("MoveNext"));
            il.Emit(OpCodes.Brtrue_S, top);

            il.MarkLabel(done);
        }

        public void Read(object source, Action<object> action) 
        {
            IEnumerator iter = _getEnumerator.Invoke(_property.GetValue(source, null), null) as IEnumerator;
            while(iter.MoveNext())
                action(iter.Current);
        }

        public void Write(object target, object value)
        {
            _add.Invoke(_property.GetValue(target, null), new object[]{ value});
        }

        public Type FieldType { get { return _add.GetParameters()[0].ParameterType; } }

        RepeatedFieldIO(PropertyInfo property, MethodInfo add, MethodInfo getEnumerator)
        {
            _property = property;
            _add = add;
            _getEnumerator = getEnumerator;
        }

        PropertyInfo _property;
        MethodInfo _add;
        MethodInfo _getEnumerator;
    }
}
