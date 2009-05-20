namespace ProtoSharp.Core.MessageFields
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    class MessageFieldString : MessageField
    {
        public MessageFieldString(int tag, IFieldIO fieldIO): base(tag, fieldIO)
        { }

        public override void AppendReadField(ILGenerator il)
        {
            il.Call<MessageReader>("ReadString");
        }

        public override void AppendGuard(ILGenerator il, MethodInfo getMethod, Label done)
        {
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Call, getMethod);
            il.Emit(OpCodes.Brfalse, done);
        }

        public override void AppendWriteField(ILGenerator il)
        {
            il.Call<MessageWriter>("WriteString", typeof(string));
        }

        public override WireType WireType
        {
            get { return WireType.String; }
        }
    }
}