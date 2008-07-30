using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    public static class Message
    {
        public static void ForEachField(Type messageType, Action<MessageField> action)
        {
            var fields = new List<MessageField>();
            Array.ForEach(messageType.GetProperties(), field =>
            {
                var attributes = field.GetCustomAttributes(typeof(TagAttribute), false);
                if(attributes.Length == 0)
                    return;
                var tag = attributes[0] as TagAttribute;
                fields.Add(MessageField.Create(tag, field));
            });
            fields.Sort((x, y) => x.Number - y.Number);
            fields.ForEach(action);
        }

        public static int CountFields(object obj)
        {
            int count = 0;
            ForEachField(obj.GetType(), x => ++count);
            return count;
        }

        public static DynamicMethod BeginWriteMethod(Type type, Type arg0)
        {
            var writer = new DynamicMethod(string.Format("DynamicWrite{0}", type.Name), null, new Type[] { arg0, typeof(MessageWriter) }, true);
            return StoreTypeSafeArg0(writer, type, arg0);
        }

        public static DynamicMethod BeginReadMethod(Type type, Type arg0)
        {
            var reader = new DynamicMethod(string.Format("DynamicRead{0}", type.Name), null, new Type[] { arg0, typeof(MessageReader) }, true);
            return StoreTypeSafeArg0(reader, type, arg0);
        }

        public static T EndMethod<T>(DynamicMethod writer) where T : class
        {
            var il = writer.GetILGenerator();
            il.Emit(OpCodes.Ret);
            return writer.CreateDelegate(typeof(T)) as T;
        }

        static DynamicMethod StoreTypeSafeArg0(DynamicMethod method, Type type, Type arg0)
        {
            var il = method.GetILGenerator();
            il.DeclareLocal(type);
            il.Emit(OpCodes.Ldarg_0);
            if(type != arg0)
                il.Emit(OpCodes.Castclass, type);
            il.Emit(OpCodes.Stloc_0);
            return method;
        }
    }
}
