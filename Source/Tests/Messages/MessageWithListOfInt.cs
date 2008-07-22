using System;
using System.Collections.Generic;
using System.Text;
using ProtoSharp.Core;

namespace ProtoSharp.Tests.Messages
{
    class MessageWithListOfInt
    {
        [Tag(1)]
        public List<int> Data { get { return _data; } }

        List<int> _data = new List<int>();
    }
}
