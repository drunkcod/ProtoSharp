using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    abstract class MessageFieldFixed<TNativeType> : MessageField
    {
        public MessageFieldFixed(int tag, IFieldIO fieldIO, WireType wireType) : base(tag, fieldIO, wireType) { }

        public override void AppendWriteField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteFixed", new Type[] { typeof(TNativeType) }));
        }
    }
}
