using System;
using System.Reflection;

namespace ProtoSharp.Core
{
    public interface  IFieldIO
    {
        void Read(object source, Action<object> action);
        void Write(object target, object value);
        Type FieldType { get; }
    }

    class FieldIO : IFieldIO
    {
        public FieldIO(PropertyInfo property)
        {
            _property = property;
        }

        public void Read(object source, Action<object> action) 
        { action(_property.GetValue(source, null)); }


        public void Write(object target, object value)
        {
            _property.SetValue(target, value, null);
        }

        public Type FieldType { get { return _property.PropertyType; } }

        PropertyInfo _property;
    }
}
