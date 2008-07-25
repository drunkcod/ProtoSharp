using System;
using ProtoSharp.Core;


namespace ProtoSharp.Tests.Messages
{
    class Test1Nullable
    {
        [Tag(1)]
        public Int32? A { get; set; }

    }
}
