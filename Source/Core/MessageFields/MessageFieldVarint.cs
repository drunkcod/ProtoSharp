using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core.MessageFields
{
    abstract class MessageFieldVarint<TNativeType>: MessageField
    {
        public MessageFieldVarint(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Varint) { }

        public override void AppendWriteField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteVarint", new Type[] { typeof(TNativeType) }));
        }
    }
}
