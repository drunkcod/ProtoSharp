using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
namespace ProtoSharp.Core
{
    using System.Reflection.Emit;
    using System.Collections;

    class PackedFieldIO : RepeatedFieldIO
    {
        public static new bool TryCreate(PropertyInfo property, out IFieldIO io) 
        {
            return TryCreate(property, add => new PackedFieldIO(property, add), out io);
        }

        protected PackedFieldIO(PropertyInfo property, MethodInfo add) : base(property, add) { }

        protected override void AppendMessageHeaderCore(ILGenerator il, ForEachLoop loop, MessageField field)
        {
            var length = il.DeclareLocal(typeof(int));
            loop.Create(body =>
            {
                body.Emit(OpCodes.Ldarg_1);
                loop.LoadCurrentAs(FieldType);
                field.AppendFieldLength(il);
                body.Emit(OpCodes.Ldloc, length.LocalIndex);
                body.Emit(OpCodes.Add);
                body.Emit(OpCodes.Stloc, length.LocalIndex);
            });

            il.Emit(OpCodes.Ldloc, length.LocalIndex);
            var done = il.DefineLabel();
            il.Emit(OpCodes.Brfalse, done);

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldc_I4, MessageTag.AsInt(field.Number, WireType.String));
            il.Call<MessageWriter>("WriteVarint", typeof(uint));
            il.Emit(OpCodes.Ldloc, length.LocalIndex);
            il.Call<MessageWriter>("WriteVarint", typeof(uint)).Pop();
            il.MarkLabel(done);
        }

        public override void AppendRead(ILGenerator il, MessageField field)
        {
            var count = il.DeclareLocal(typeof(uint));
            var top = il.DefineLabel();
            var next = il.DefineLabel();

            il.Emit(OpCodes.Br_S, next);
            il.Emit(OpCodes.Nop);
            il.MarkLabel(top);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Call, property.GetGetMethod());
            il.Emit(OpCodes.Ldarg_1);
            field.AppendReadField(il);
            il.Emit(OpCodes.Callvirt, addItem);
            il.MarkLabel(next);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Call, typeof(MessageReader).GetProperty("EndOfStream").GetGetMethod());
            il.Emit(OpCodes.Brfalse_S, top);
        }
    }
}