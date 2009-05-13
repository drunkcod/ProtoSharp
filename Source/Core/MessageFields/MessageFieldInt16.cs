namespace ProtoSharp.Core.MessageFields
{
    using System;
    using System.Reflection.Emit;

    class MessageFieldInt16 : MessageField
    {
        public MessageFieldInt16(int tag, IFieldIO fieldIO) : base(tag, fieldIO) { }

        public override void AppendWriteField(ILGenerator il)
        {
            il.Emit(OpCodes.Conv_I4);
            il.Call<MessageWriter>("WriteVarint", typeof(int));
        }

        public override void AppendReadField(ILGenerator il)
        {
            il.Call<MessageReader>("ReadInt32");
        }
    }
}
