using System;
using System.Reflection.Emit;
using System.Reflection;

namespace ProtoSharp.Core.MessageFields
{
    class MessageFieldEnum : MessageFieldVarint<int>
    {
        public MessageFieldEnum(int tag, IFieldIO fieldIO) : base(tag, fieldIO) { }

        public override void AppendReadField(ILGenerator il)
        {
            il.Emit(OpCodes.Ldtoken, FieldType);
            il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
            il.Emit(OpCodes.Call, typeof(MessageReader).GetMethod("ReadEnum", new Type[]{ typeof(Type) }));
        }
    }
}
