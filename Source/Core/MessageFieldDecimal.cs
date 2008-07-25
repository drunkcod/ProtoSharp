using System;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    public class MessageFieldDecimal : MessageField
    {
        public const int Factor = 10000;

        public MessageFieldDecimal(int tag, IFieldIO fieldIO) : base(tag, fieldIO, WireType.Varint) { }

        protected override bool CanAppendWriteCore { get { return true; } }
        protected override bool CanAppendReadCore { get { return true; } }

        public override void AppendWriteField(ILGenerator il)
        {
            il.Emit(OpCodes.Ldc_I4, Factor);
            il.Emit(OpCodes.Newobj, typeof(Decimal).GetConstructor(new Type[] { typeof(int) }));
            il.Emit(OpCodes.Call, typeof(Decimal).GetMethod("Multiply"));
            il.Emit(OpCodes.Call, typeof(Decimal).GetMethod("ToInt64"));
            il.Emit(OpCodes.Call, typeof(MessageWriter).GetMethod("WriteVarint", new Type[] { typeof(Int64) }));
        }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadVarint64", Type.EmptyTypes));
            il.Emit(OpCodes.Newobj, typeof(Decimal).GetConstructor(new Type[] { typeof(Int64) }));
            il.Emit(OpCodes.Ldc_I4, Factor);
            il.Emit(OpCodes.Newobj, typeof(Decimal).GetConstructor(new Type[] { typeof(int) }));
            il.Emit(OpCodes.Call, typeof(Decimal).GetMethod("Divide"));
        }

        protected override object DoRead(MessageReader reader)
        {
            return new Decimal(reader.ReadVarint64());
        }
    }
}
