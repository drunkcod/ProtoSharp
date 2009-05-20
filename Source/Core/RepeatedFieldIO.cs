using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    public class RepeatedFieldIO : FieldIOBase
    {
        protected MethodInfo addItem;
        
        public static bool TryCreate(PropertyInfo property, out IFieldIO io)
        {
            return TryCreate(property, add => new RepeatedFieldIO(property, add), out io);
        }

        protected static bool TryCreate(PropertyInfo property, Func<MethodInfo, IFieldIO> create, out IFieldIO io)
        {
            var add = property.PropertyType.GetMethod("Add");
            var getEnumerator = property.PropertyType.GetMethod("GetEnumerator");
            if (add == null || getEnumerator == null)
            {
                io = null;
                return false;
            }
            io = create(add);
            return true;
        }

        protected RepeatedFieldIO(PropertyInfo property, MethodInfo addItem)
            : base(property)
        {
            this.addItem = addItem;
        }

        public override Type FieldType { get { return addItem.GetParameters()[0].ParameterType; } }

        public override void AppendWrite(ILGenerator il, MessageField field)
        {
            var loop = new ForEachLoop(il, property);
            AppendMessageHeaderCore(il, loop, field);
            loop.Create(body =>
            {
                field.AppendHeader(il);
                loop.LoadCurrentAs(FieldType);
                field.AppendWriteField(il);
                body.Emit(OpCodes.Pop);
            });
        }

        protected virtual void AppendMessageHeaderCore(ILGenerator il, ForEachLoop loop, MessageField field) { }

        public override void AppendRead(ILGenerator il, MessageField field)
        {
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Call, property.GetGetMethod());

            il.Emit(OpCodes.Ldarg_1);
            field.AppendReadField(il);

            il.Emit(OpCodes.Callvirt, addItem);
        }

        //foreach(var item in collection)...
        protected class ForEachLoop
        {
            readonly ILGenerator il;
            readonly MethodInfo getField;
            readonly MethodInfo getEnumerator;
            LocalBuilder enumerator;
            Label next;
            Label top;
            bool EnumeratorCreated { get { return enumerator != null; } }

            public ForEachLoop(ILGenerator il, PropertyInfo property)
            {
                this.il = il;
                this.getEnumerator = property.PropertyType.GetMethod("GetEnumerator");
                this.getField = property.GetGetMethod();
            }

            Type GetEnumeratorType()
            {
                if (typeof(IEnumerator<>).IsAssignableFrom(getEnumerator.ReturnType))
                    return getEnumerator.ReturnType;
                return typeof(IEnumerator);
            }

            public void Create(Action<ILGenerator> writeBody)
            {
                Begin();
                writeBody(il);
                End();
            }

            void Begin()
            {
                next = il.DefineLabel();
                top = il.DefineLabel();
                if (EnumeratorCreated)
                    ResetEnumerator();
                else
                    CreateEnumerator();
                il.Emit(OpCodes.Br, next);
                il.MarkLabel(top);
            }

            void End()
            {
                il.MarkLabel(next);
                il.Emit(OpCodes.Ldloc, enumerator);
                il.Emit(OpCodes.Callvirt, typeof(IEnumerator).GetMethod("MoveNext"));
                il.Emit(OpCodes.Brtrue_S, top);
            }

            private void ResetEnumerator()
            {
                il.Emit(OpCodes.Ldloc, enumerator);
                il.Emit(OpCodes.Callvirt, typeof(IEnumerator).GetMethod("Reset"));
            }

            void CreateEnumerator()
            {
                enumerator = il.DeclareLocal(GetEnumeratorType());
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Call, getField);
                il.Emit(OpCodes.Call, getEnumerator);
                if(getEnumerator.ReturnType.IsValueType)
                    il.Emit(OpCodes.Box, getEnumerator.ReturnType);
                il.Emit(OpCodes.Stloc, enumerator);
            }

            public void LoadCurrentAs(Type fieldType)
            {
                il.Emit(OpCodes.Ldloc, enumerator.LocalIndex);
                il.Emit(OpCodes.Callvirt, GetEnumeratorType().GetProperty("Current").GetGetMethod());
                if (GetEnumeratorType() == typeof(IEnumerator))
                    il.Emit(OpCodes.Unbox_Any, fieldType);
            }
        }
    }
}