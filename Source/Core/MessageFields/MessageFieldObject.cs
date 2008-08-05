using System;
using System.Reflection.Emit;
using System.Reflection;

namespace ProtoSharp.Core.MessageFields
{
    class MessageFieldObject: MessageField
    {
        public MessageFieldObject(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.String) { }

        public override void AppendWriteField(ILGenerator il)
        {
            il.Emit(OpCodes.Ldc_I4, Number);
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteObject").MakeGenericMethod(FieldType));
        }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("Read", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(FieldType));
        }

        public override void AppendHeader(ILGenerator il)
        {
            il.Emit(OpCodes.Ldarg_1);
        }
    }
}
