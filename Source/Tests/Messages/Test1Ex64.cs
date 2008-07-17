using System;
using ProtoSharp.Core;

namespace ProtoSharp.Tests.Messages
{
    class Test1Ex64
    {
        [Tag(1)]
        public Int64 A { get { return _a; } set { _a = value; } }

        Int64 _a;
    }
}
