namespace ProtoSharp.Core
{
    using System;

    class UnknownEnumException : ArgumentOutOfRangeException
    {
        public UnknownEnumException(int value)
        {
            _value = value;
        }

        public int Value { get { return _value; } }

        int _value;
    }
}
