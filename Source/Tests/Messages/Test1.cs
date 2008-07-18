using System;
using ProtoSharp.Core;


namespace ProtoSharp.Tests.Messages
{
    class Test1
    {
        [Tag(1)]
        public Int32 A { get { return _a; } set { _a = value; } }

        int _a;
    }
}
