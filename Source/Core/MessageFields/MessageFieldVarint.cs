namespace ProtoSharp.Core.MessageFields
{
    using System;
    using System.Reflection.Emit;

    class MessageFieldVarint : MessageField
    {
        public MessageFieldVarint(int tag, IFieldIO fieldIO) : base(tag, fieldIO) { }

        public override void AppendWriteField(ILGenerator il)
        {
            il.Call<MessageWriter>("WriteVarint", FieldType);
        }
    }
}
