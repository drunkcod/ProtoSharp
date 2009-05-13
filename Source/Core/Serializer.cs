using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;
using System.IO;
using ProtoSharp.Core.UnknownFields;

namespace ProtoSharp.Core
{
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

        public static void Serialize<T>(MessageWriter writer, T value)
        {
            Serializer<T>.FieldWriter(value, writer);
        }

        public static void Serialize<T>(Stream stream, T value)
        {
            Serialize(new MessageWriter(stream), value);
        }

        public static T Deserialize<T>(Stream stream, UnknownFieldCollection missing) where T : new()
        {
            return Deserialize<T>(new MessageReader(stream), missing);
        }

        public static T Deserialize<T>(Stream stream) where T : new()
        {
            return Deserialize<T>(new MessageReader(stream), new UnknownFieldCollection());
        }

        public static T Deserialize<T>(MessageReader reader, UnknownFieldCollection missing) where T : new()
        {
            return new Serializer<T>().Deserialize(reader, new T(), missing);
        }

        public static T Deserialize<T>(MessageReader reader) where T : new()
        {
            return new Serializer<T>().Deserialize(reader, new T(), new UnknownFieldCollection());
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

        public bool TryGetFieldReader(MessageTag tag)
        {
            if(FindReader(tag.Value))
                return true;
            return tag.WireType == WireType.StartGroup && FindReader(tag.WithWireType(WireType.String));
        }
        
        static readonly KeyValuePair<int, FieldReader<T>>[] s_fields =  GetFields();

        public static readonly FieldWriter<T> FieldWriter = Message.CreateFieldWriter<T>();

        public T Deserialize(MessageReader reader, T target, UnknownFieldCollection missing)
        {
            MessageTag tag = new MessageTag();
            while(reader.TryReadMessageTag(ref tag))
            {
                if(TryGetFieldReader(tag))
                {
                    if(tag.WireType == WireType.String)
                        _current.Value(target, reader.CreateSubReader());
                    else
                        try
                        {
                            _current.Value(target, reader);
                        }
                        catch(UnknownEnumException e)
                        {
                            missing.Add(new UnknownFieldVarint(tag, e.Value));
                        }
                }
                else if(tag.WireType == WireType.EndGroup)
                    break;
                else if(tag.WireType < WireType.MaxValid)
                    missing.Add(tag, reader);
                else
                    throw new NotSupportedException();
            }
            return target;
        }

        //Optimized for the common case where fields are seen in increasing number order.
        bool FindReader(int header)
        {
            if(header == _current.Key)
                return true;
            if(header > _current.Key)
            {
                for(int i = _position + 1; i != s_fields.Length; ++i)
                {
                    var current = s_fields[i];
                    if(current.Key == header)
                    {
                        _position = i;
                        _current = current;
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
                    return true;
                }
            }
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

        int _position = -1;
        KeyValuePair<int, FieldReader<T>> _current;
    }
}
