namespace ProtoSharp.Core.MessageFields
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    class MessageFieldObject : MessageField
    {
        public MessageFieldObject(int tag, IFieldIO fieldIO) : base(tag, fieldIO) { }

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

        public override WireType WireType
        {
            get { return WireType.String; }
        }
    }
}
