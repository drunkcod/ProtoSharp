using System;
using System.Collections.Generic;
using System.Text;

namespace ProtoSharp.Core
{
    public class DefaultAttribute : Attribute
    {
        public DefaultAttribute(string value)
        {
            _value = value;
        }

        public string Value { get { return _value; } }

        string _value;
    }
}
