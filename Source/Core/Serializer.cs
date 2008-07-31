using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;
using System.IO;

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

    class SerializerHelper<T>
    {
        public bool TryGetFieldReader(MessageTag tag, out FieldReader<T> reader)
        {
            if(FindReader(tag.Value, out reader))
                return true;
            return tag.WireType == WireType.StartGroup && FindReader(tag.WithWireType(WireType.String), out reader);
        }
        
        static readonly KeyValuePair<int, FieldReader<T>>[] s_fields =  GetFields();

        public static readonly FieldWriter<T> FieldWriter = GetWriter();

        //Optimized for the common case where fields are seen in incresing number order.
        bool FindReader(int header, out FieldReader<T> reader)
        {
            if(header == _current.Key)
            {
                reader = _current.Value;
                return true;
            }
            if(header > _current.Key)
            {
                for(int i = _position + 1; i != s_fields.Length; ++i)
                {
                    var current = s_fields[i];
                    if(current.Key == header)
                    {
                        _position = i;
                        _current = current;
                        reader = current.Value;
                        return true;
                    }
                }
            }
            for(int i = _position; i-- > 0;)
            {
                var current = s_fields[i];
                if(current.Key == header)
                {
                    _position = i;
                    _current = current;
                    reader = current.Value;
                    return true;
                }
            }
            reader = null;
            return false;
        }

        static KeyValuePair<int, FieldReader<T>>[] GetFields()
        {
            var fields = new List<KeyValuePair<int, FieldReader<T>>>();
            Message.ForEachField(typeof(T),
                field => fields.Add(new KeyValuePair<int, FieldReader<T>>(field.Header, field.GetFieldReader<T>())));
            fields.Sort((x, y) => x.Key - y.Key);
            return fields.ToArray();
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
            fieldWriter += Message.EndMethod<FieldWriter<T>>(writer);
            return fieldWriter;
        }

        int _position = -1;
        KeyValuePair<int, FieldReader<T>> _current;
    }
}
