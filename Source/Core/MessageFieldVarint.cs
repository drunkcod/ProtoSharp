using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    abstract class MessageFieldVarint<TNativeType>: MessageField
    {
        public MessageFieldVarint(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Varint) { }

        public override bool AppendWrite(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteVarint", new Type[] { typeof(TNativeType) }));
            return true;
        }
    }
}
