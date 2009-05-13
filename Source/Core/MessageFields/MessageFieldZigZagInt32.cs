namespace ProtoSharp.Core.MessageFields
{
    using System;
    using System.Reflection.Emit;

    class MessageFieldZigZagInt32 : MessageField
    {
        public MessageFieldZigZagInt32(int tag, IFieldIO fieldIO): base(tag, fieldIO){}

        public override void AppendWriteField(ILGenerator il)
        {
            il.Call<MessageWriter>("WriteZigZag", FieldType);
        }

        public override void AppendReadField(ILGenerator il)
        {
            il.Call<MessageReader>("ReadZigZag32");
        }
    }
}