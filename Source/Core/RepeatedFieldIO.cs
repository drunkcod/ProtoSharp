using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    public class RepeatedFieldIO : FieldIOBase
    {
        public override Type FieldType { get { return _add.GetParameters()[0].ParameterType; } }

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

        public override void AppendWrite(ILGenerator il, MessageField field)
        {
            var done = il.DefineLabel();
            var top = il.DefineLabel();
            var next = il.DefineLabel();

            var enumeratorType = GetEnumeratorType();

            var enumerator = il.DeclareLocal(enumeratorType);

            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Call, _property.GetGetMethod());
            il.Emit(OpCodes.Call, _getEnumerator);
            il.Emit(OpCodes.Box, _getEnumerator.ReturnType);
            il.Emit(OpCodes.Stloc, enumerator.LocalIndex);
            il.Emit(OpCodes.Br, next);
            il.MarkLabel(top);

            AppendWriteHeader(il, field);

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldloc, enumerator.LocalIndex);
            il.Emit(OpCodes.Callvirt, enumeratorType.GetProperty("Current").GetGetMethod());
            if(enumeratorType == typeof(IEnumerator))
                il.Emit(OpCodes.Unbox_Any, FieldType);
            field.AppendWriteField(il);

            il.MarkLabel(next);
            il.Emit(OpCodes.Ldloc, enumerator.LocalIndex);
            il.Emit(OpCodes.Callvirt, typeof(IEnumerator).GetMethod("MoveNext"));
            il.Emit(OpCodes.Brtrue_S, top);

            il.MarkLabel(done);
        }

        public override void Read(object source, Action<object> action) 
        {
            IEnumerator iter = _getEnumerator.Invoke(_property.GetValue(source, null), null) as IEnumerator;
            while(iter.MoveNext())
                action(iter.Current);
        }

        public override void Write(object target, object value)
        {
            _add.Invoke(_property.GetValue(target, null), new object[]{ value});
        }

        RepeatedFieldIO(PropertyInfo property, MethodInfo add, MethodInfo getEnumerator) : base(property)
        {
            _add = add;
            _getEnumerator = getEnumerator;
        }

        Type GetEnumeratorType()
        {
            var enumeratorOfT = typeof(IEnumerator<>).MakeGenericType(FieldType);
            if(enumeratorOfT.IsAssignableFrom(_getEnumerator.ReturnType))
                return enumeratorOfT;
            return typeof(IEnumerator);
        }

        MethodInfo _add;
        MethodInfo _getEnumerator;
    }
}
