using System;
using System.Collections.Generic;
using ProtoSharp.Core;

namespace ProtoSharp.Tests.Messages
{
    class WithRepeatedTest1
    {
        [Tag(2)]
        public List<Test1> Data { get { return _data; } }

        List<Test1> _data = new List<Test1>();
    }
}
