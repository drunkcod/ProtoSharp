namespace ProtoSharp.Core.MessageFields
{
    using System;
    using System.Reflection.Emit;

    class MessageFieldEnum : MessageField
    {
        public MessageFieldEnum(int tag, IFieldIO fieldIO) : base(tag, fieldIO) { }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Ldtoken, FieldType);
            il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
            il.Call<MessageReader>("ReadEnum", typeof(Type));
        }

        public override void AppendWriteField(ILGenerator il)
        {
            il.Call<MessageWriter>("WriteVarint", typeof(int));
        }
    }
}