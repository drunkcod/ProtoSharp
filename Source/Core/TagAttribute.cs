using System;

namespace ProtoSharp.Core
{
    public class TagAttribute : Attribute
    {
        public TagAttribute(int number)
        {
            _number = number;
        }

        public int Number { get { return _number; } }
        public bool UseFixed { get { return _useFixed; } set { _useFixed = value; } }
        public bool UseZigZag { get { return _useZigZag; } set { _useZigZag = value; } }

        int _number;
        bool _useFixed;
        bool _useZigZag;
    }
}
