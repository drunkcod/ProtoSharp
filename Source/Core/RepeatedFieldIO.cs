using System;
using System.Collections;
using System.Reflection;

namespace ProtoSharp.Core
{
    public class RepeatedFieldIO : IFieldIO
    {
        public static bool TryCreate(PropertyInfo property, out IFieldIO io)
        {
            MethodInfo add = property.PropertyType.GetMethod("Add");
            MethodInfo getEnumerator = property.PropertyType.GetMethod("GetEnumerator", Type.EmptyTypes);
            if(add == null || getEnumerator == null)
            {
                io = null;
                return false;
            }
            io = new RepeatedFieldIO(property, add, getEnumerator);
            return true;
        }

        private RepeatedFieldIO(PropertyInfo property, MethodInfo add, MethodInfo getEnumerator)
        {
            _property = property;
            _add = add;
            _getEnumerator = getEnumerator;
        }

        public void Read(object source, Action<object> action) 
        {
            IEnumerator iter = _getEnumerator.Invoke(_property.GetValue(source, null), null) as IEnumerator;
            while(iter.MoveNext())
                action(iter.Current);
        }

        public void Write(object target, object value)
        {
            _add.Invoke(_property.GetValue(target, null), new object[]{ value});
        }

        public Type FieldType { get { return _add.GetParameters()[0].ParameterType; } }

        PropertyInfo _property;
        MethodInfo _add;
        MethodInfo _getEnumerator;
    }
}
