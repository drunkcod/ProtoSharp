using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core.MessageFields
{
    class MessageFieldFixed : MessageField
    {
        readonly WireType wireType;

        public MessageFieldFixed(int tag, IFieldIO fieldIO, WireType wireType) : base(tag, fieldIO)
        {
            this.wireType = wireType;
        }

        public override void AppendWriteField(ILGenerator il)
        {
            il.Call<MessageWriter>("WriteFixed", FieldType);
        }

        public override void AppendReadField(ILGenerator il)
        {
            il.Call<MessageReader>("ReadFixed" + FieldType.Name);
        }

        protected override WireType WireType
        {
            get { return wireType; }
        }
    }
}
