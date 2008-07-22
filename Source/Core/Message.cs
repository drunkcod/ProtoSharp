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
            Array.ForEach(messageType.GetProperties(), field =>
            {
                var attributes = field.GetCustomAttributes(typeof(TagAttribute), false);
                if(attributes.Length == 0)
                    return;
                var tag = attributes[0] as TagAttribute;
                action(MessageField.Create(tag, field));
            });
        }

        public static int CountFields(object obj)
        {
            int count = 0;
            ForEachField(obj.GetType(), x => ++count);
            return count;
        }

        public static T CreateDefault<T>() where T : class, new()
        {
            return CreateDefault(new T()) as T;
        }

        public static T CreateDefaultItem<T>(string s)
        {
            return (T)CreateDefaultItem(typeof(T), s);
        }

        public static DynamicMethod BeginWriteMethod(Type type)
        {
            var writer = new DynamicMethod(string.Format("DynamicWrite{0}", type.Name), null, new Type[] { typeof(object), typeof(MessageWriter) }, true);
            var il = writer.GetILGenerator();
            il.DeclareLocal(type);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, type);
            il.Emit(OpCodes.Stloc_0);

            return writer;
        }

        public static DynamicMethod BeginReadMethod(Type type)
        {
            var reader = new DynamicMethod(string.Format("DynamicRead{0}", type.Name), null, new Type[] { typeof(object), typeof(MessageReader) }, true);
            var il = reader.GetILGenerator();
            il.DeclareLocal(type);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, type);
            il.Emit(OpCodes.Stloc_0);

            return reader;
        }

        public static FieldWriter EndWriteMethod(DynamicMethod writer)
        {
            var il = writer.GetILGenerator();
            il.Emit(OpCodes.Ret);
            return writer.CreateDelegate(typeof(FieldWriter)) as FieldWriter;
        }

        public static FieldReader EndReadMethod(DynamicMethod reader)
        {
            var il = reader.GetILGenerator();
            il.Emit(OpCodes.Ret);
            return reader.CreateDelegate(typeof(FieldReader)) as FieldReader;
        }

        static object CreateDefault(object obj)
        {
            Array.ForEach(obj.GetType().GetProperties(), item =>
            {
                var attributes = item.GetCustomAttributes(typeof(DefaultAttribute), false);
                if(attributes.Length == 0)
                    return;
                var defaultAttribute = attributes[0] as DefaultAttribute;
                item.SetValue(obj, CreateDefaultItem(item.PropertyType, defaultAttribute.Value), null);
            });
            return obj;
        }
        static object CreateDefaultItem(Type type, string s)
        {
            if(type.IsEnum)
                return Enum.Parse(type, s);

            var stringMethodArg = new Type[] { typeof(string) };

            var parse = type.GetMethod("Parse", stringMethodArg);
            if(parse != null)
                return parse.Invoke(null, new object[] { s });

            var tryParse = type.GetMethod("TryParse", new Type[]{ typeof(string), type.MakeByRefType() });
            if(tryParse != null)
            {
                var args = new object[] { s, null };
                if((bool)tryParse.Invoke(null, args))
                    return args[1];
            }

            var constructor = type.GetConstructor(stringMethodArg);
            if(constructor != null)
                return constructor.Invoke(new object[] { s });

            return null;
        }
    }
}
