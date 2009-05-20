namespace ProtoSharp.Core.MessageFields
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    class MessageFieldPacked : MessageField
    {
        MessageField parent;
        public MessageFieldPacked(MessageField parent) : base(parent) 
        {
            if (!parent.WireType.EqualsAny(WireType.Varint, WireType.Fixed32, WireType.Fixed64))
                throw new NotSupportedException();

            this.parent = parent;
        }

        public override WireType WireType { get { return WireType.String; } }

        public override void AppendWriteField(ILGenerator il)
        {
            parent.AppendWriteField(il);
        }

        public override void AppendFieldLength(ILGenerator il)
        {
            parent.AppendFieldLength(il);
        }

        public override void AppendReadField(ILGenerator il)
        {
            parent.AppendReadField(il);
        }

        public override void AppendGuard(ILGenerator il, MethodInfo getMethod, Label done)
        {
            parent.AppendGuard(il, getMethod, done);
        }

        public override void AppendHeader(System.Reflection.Emit.ILGenerator il)
        {
            il.Emit(OpCodes.Ldarg_1);
        }
    }
}
