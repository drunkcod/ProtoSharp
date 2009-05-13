namespace ProtoSharp.Core.MessageFields
{
    using System;
    using System.Reflection.Emit;

    class MessageFieldBytes : MessageField
    {
        public MessageFieldBytes(int tag, IFieldIO fieldIO) : base(tag, fieldIO) { }

        public override void AppendWriteField(ILGenerator il)
        {
            il.Call<MessageWriter>("WriteBytes", typeof(byte[]));
        }

        public override void AppendReadField(ILGenerator il)
        {
            il.Call<MessageReader>("ReadBytes");
        }
    }
}
