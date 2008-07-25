using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;

namespace ProtoSharp.Core
{
    interface ISerializer<T>
    {
        T CreateDefault();
    }

    public static class Serializer
    {
        public static T CreateDefault<T>() where T : class, new()
        {
            return CreateDefault(new T()) as T;
        }

        public static T CreateDefaultItem<T>(string s)
        {
            return (T)CreateDefaultItem(typeof(T), s);
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

            var tryParse = type.GetMethod("TryParse", new Type[] { typeof(string), type.MakeByRefType() });
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

    class Serializer<T>
    {
        public static readonly Dictionary<int, FieldReader<T>> Fields = GetFields();
        public static readonly FieldWriter<T> FieldWriter = GetWriter();

        static Dictionary<int, FieldReader<T>> GetFields()
        {
            var fields = new Dictionary<int, FieldReader<T>>();
            Message.ForEachField(typeof(T),
                field => fields.Add(field.Tag, field.GetFieldReader<T>()));
            return fields;
        }

        static FieldWriter<T> GetWriter()
        {
            FieldWriter<T> fieldWriter = null;
            var writer = Message.BeginWriteMethod(typeof(T), typeof(T));
            Message.ForEachField(typeof(T), x =>
            {
                if(x.CanAppendWrite)
                    x.AppendWriteBody(writer.GetILGenerator());
                else
                    throw new NotSupportedException();
            });
            fieldWriter += Message.EndWriteMethod<FieldWriter<T>>(writer);
            return fieldWriter;
        }
    }
}
