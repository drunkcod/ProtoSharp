using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    class MessageFieldSInt32 : MessageField
    {
        public MessageFieldSInt32(int tag, IFieldIO fieldIO): base(tag, fieldIO, WireType.Varint){}

        protected override bool CanAppendWriteCore { get { return true; } }

        public override void AppendWriteField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteZigZag", new Type[] { typeof(int) }));
        }

        protected override object DoRead(MessageReader reader)
        {
            return reader.ReadZigZag32();
        }
    }
}
